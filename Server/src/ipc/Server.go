package ipc

import (
	"net"
	"log"
	"encoding/json"
	"DB"
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
}
type Account struct {
	User string
	Pass string
}
func NewIpcServer(name string,DB *DB.DBTool)*IpcServer{

	return &IpcServer{Name:name,DBtool:DB}
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
		err =json.Unmarshal([]byte(req.Params),&ac)
		if err!=nil{
			log.Println("err in handle with act login ")
		}
		rep ,err =server.handleLogin(ac.User,ac.Pass)
		if err!=nil{
			log.Println("err in handle with act login ")
		}
		//return rep,nil
	case "REG":
		var peopleid string = req.Params
		rep,err =server.handleReg(peopleid)
		if err!=nil{
			log.Println("err in handle reg")
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
	username,_,err:=server.DBtool.Reg(peopleid)
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

