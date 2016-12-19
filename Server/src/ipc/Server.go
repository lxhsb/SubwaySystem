package ipc

import (
	"net"
	"log"
	"encoding/json"
	"DB"
	"time"
	"sync"
	"fmt"
)

type Request struct {
	Method string
	Params string
}
type Response struct {
	Code string
	Body string
}
type IpcServer struct {
	Name string //server name
	DBtool *DB.DBTool
	Lock *sync.RWMutex
	Ynum int
	Tnum int
}
type Account struct {//这个是用来登录的
	User string
	Pass string
}
type User struct {//这个是实际用户
	Cardid string
	Peopleid string
	Money int
}
func NewIpcServer(name string,DB *DB.DBTool,lock *sync.RWMutex)*IpcServer{
	var ans = &IpcServer{Name:name,DBtool:DB,Lock:lock}
	go ans.Refresh()
	return ans;
}

func (server *IpcServer)Refresh(){
	select {
	case <-time.After(time.Second):
		if time.Now().Hour()==0&&time.Now().Minute()==0&&time.Now().Minute()==0{
			server.Lock.Lock()
			defer server.Lock.Unlock()
			server.Ynum=0;
			server.Tnum=0;
		}
	}
}
func (server *IpcServer)getUserName(kind string)(string){
	server.Lock.RLock()
	defer server.Lock.RUnlock()

	if kind == "Y"{
		server.Ynum++;
		return fmt.Sprintf("Y%d%03d%07d",time.Now().Year(),time.Now().YearDay(),server.Ynum)
	}else {
		server.Tnum++;
		return fmt.Sprintf("Y%d%03d%07d",time.Now().Year(),time.Now().YearDay(),server.Tnum)
	}

}
func (server *IpcServer)ReceiveMessage(conn net.Conn){//这点可能会出bug 待修改

	buf := make([]byte ,1<<21)
	for{
		len,err:=conn.Read(buf)
		if err!=nil{
			log.Println("err receive from" + conn.RemoteAddr().String())
			break
		}
		var req Request
		err=json.Unmarshal(buf[0:len],&req)

		if err!=nil{//这点先这样写
			log.Println("err unmarshal json from " + conn.RemoteAddr().String())
			break
		}
		rep,err:= server.handle(req)
		server.send(rep,conn)
	}
}
func (server *IpcServer)send(rep Response,conn net.Conn)(error){

	b,err:= json.Marshal(rep)
	if err!=nil{
		return err
	}
	conn.Write(b)
	return nil

}
func (server *IpcServer)handle(req Request)(Response,error){
	var rep Response
	var err error = nil
	switch req.Method {
	case "LOGIN":
		var ac Account
		err =json.Unmarshal([]byte(req.Params),&ac)//要对body进行 解码
		if err!=nil{
			log.Println("err in handle with act login ")
		}
		rep ,err =server.handleLogin(ac.User,ac.Pass)
		if err!=nil{
			log.Println("err in handle with act login ")
		}
	case "REG":
		var peopleid string = req.Params
		rep,err =server.handleReg(peopleid)
		if err!=nil{
			log.Println("err in handle reg")
		}
	case "GETALLFREQUENTUSERLIST"://虽然很奇怪这样 但是还是统一一下
		rep,err = server.handleGetAllFrequentUserList()
		if err!=nil{
			log.Println("err in get frequent user list")
		}
	default:
	}
	return rep,nil
}
func (server *IpcServer)handleLogin(usr ,pass string)(Response,error){//待修改
	log.Println(usr+" try to  login with pass "+ pass)
	var rep Response
	rep.Code="200"
	 realpass ,err:= server.DBtool.GetPass(usr)
	if err!=nil{
		log.Println(err)
		return rep,nil
	}
	log.Println("real pass is "+realpass)
	if realpass == pass{
		rep.Body="YES"
	}else {
		rep.Body="NO"
	}
	return rep,nil
}
func (server *IpcServer)handleReg(peopleid string)(Response,error){
	YUserName := server.getUserName("Y")
	username,_,err:=server.DBtool.Reg(peopleid,YUserName)
	var rep Response
	rep.Code = "200"
	if err!=nil{
		rep.Body = err.Error()
		log.Println("reg failesd")
	}else {
		rep.Body = "success reg username is " + username
		log.Println("reg success with username "+ username)
	}
	return rep,err
}
func (server *IpcServer)handleGetAllFrequentUserList()(Response,error){
	log.Println("getting frequent user list")
	var rep Response;
	var userlist []User
	var tmp User
	rows,err:= server.DBtool.GetAllFrequentUser()
	if err!=nil{
		log.Println(err.Error())
		return rep,nil
	}
	for rows.Next(){
		rows.Scan(&tmp.Cardid,&tmp.Peopleid,&tmp.Money)
		userlist = append(userlist,tmp)
	}
	b,err:=json.Marshal(userlist)
	if err!=nil{
		log.Println(err.Error())
		return rep,nil
	}
	rep.Code = "200"
	rep.Body = string(b[:]);
	return rep,nil
}
