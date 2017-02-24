package Servers

import (
	"DBOP"
	"sync"
	"time"
	"fmt"
	"encoding/json"
	"log"
	"strconv"
)
//CenterServer 主要负责处理实际的业务
type CenterServer struct {
	DBTool *DBOP.DBTool
	Lock *sync.RWMutex
	Ynum int
	Tnum int
}
func NewCenterServer(dbt *DBOP.DBTool) *CenterServer{
	var ans =&CenterServer{}
	ans.DBTool = dbt
	ans.Lock = &sync.RWMutex{}
	go ans.Refresh()
	return  ans
}
func (server *CenterServer)Refresh(){ //用于每日更新 之后可以在此扩展log到每日文件
	select {
	case <-time.After(time.Second):
		if time.Now().Hour()==0&&time.Now().Minute()==0&&time.Now().Second()==0{
			server.Lock.Lock()
			defer server.Lock.Unlock()
			server.Ynum=0;
			server.Tnum=0;
		}
	}
}
func (server *CenterServer)getUserName(kind string)(string){//用来获取卡号
	server.Lock.Lock()
	defer server.Lock.Unlock()
	if kind == "Y"{
		server.Ynum++;
		return fmt.Sprintf("Y%d%03d%07d",time.Now().Year(),time.Now().YearDay(),server.Ynum)
	}else {
		server.Tnum++;
		return fmt.Sprintf("T%d%03d%07d",time.Now().Year(),time.Now().YearDay(),server.Tnum)
	}

}
func (server *CenterServer)Handle(Method,Params string)(*Response,error){//实际作用是分发
	var rep *Response
	var err error = nil
	switch Method {
	case "LOGIN":
		var ac Account
		err =json.Unmarshal([]byte(Params),&ac)//要对body进行 解码
		if err!=nil{
			log.Println("err in handle with act login ")
			break
		}
		rep,err =server.handleLogin(ac.User,ac.Pass)
		if err!=nil{
			log.Println("err in handle with act login ")
			break
		}
		log.Println(ac.User +"try to get in and "+rep.Body)
	case "REG":
		var peopleid string = Params
		rep,err =server.handleReg(peopleid)
		if err!=nil{
			log.Println("err in handle reg")
			break
		}
		log.Println(rep.Body + " reg success")
	case "GETALLFREQUENTUSERLIST"://虽然很奇怪这样 但是还是统一一下
		rep,err = server.handleGetAllFrequentUserList()
		if err!=nil{
			log.Println("err in get frequent user list")
			break
		}
		log.Println("get all frequent user list");
	case "ADDMONEY":
		var addmoneyop AddMoneyOP
		err = json.Unmarshal([]byte(Params),&addmoneyop)
		if err!=nil{
			log.Println("err unmarshal json in addmoney")
			break
		}
		rep ,err = server.handleAddMoney(addmoneyop.Cardid,addmoneyop.Money)
		if err!=nil{
			log.Println("err in handle add money")
			break
		}
		log.Println(addmoneyop.Cardid+"  add money "+ strconv.Itoa(addmoneyop.Money))
	case "DELUSER":
		id :=Params
		rep,err = server.handleDelUser(id)
		if err!=nil{
			log.Println("err in handle deluser")
			break
		}
		log.Println("delete user success "+id )
	case "ASKTEMPUSER":
		str:=Params
		var tmp  AskTempRegOP
		err =json.Unmarshal([]byte(str),&tmp)
		if err!=nil{
			log.Println("err in ask temp user unmarshal json")
			break
		}
		rep,err= server.handleAskTempUser(tmp.Num,tmp.Money)
		if err!=nil{
			log.Println("err in ask temp user ")
			log.Println(err)
			break
		}
		log.Println("reg "+ strconv.Itoa(tmp.Num)+"  with money "+ strconv.Itoa(tmp.Money))
	case "GETOUT":
		str:=Params
		var tmp GetOutOP
		err = json.Unmarshal([]byte(str),&tmp)
		if err!=nil{
			log.Println("err in getout unmarshal json")
			break
		}
		//log.Println(tmp.cardid+" ")
		rep,err = server.handleGetOut(tmp.Cardid,tmp.Price)
		if err!=nil{
			log.Println("err in handle getout "+ err.Error())
			break
		}
		log.Println(tmp.Cardid+ "try to get out and result is "+ rep.Body)

	default:
		rep.Code = "404"
		rep.Body = "err"
	}
	return rep,nil
}
func (server *CenterServer)handleLogin(usr,pass string)(*Response,error){//待修改
	log.Println(usr+" try to  login with pass "+ pass)
	var rep Response
	rep.Code="200"
	realpass ,err:= server.DBTool.GetPass(usr)
	if err!=nil{
		log.Println(err)
		return &rep,nil
	}
	if realpass == pass{
		rep.Body="YES"
	}else {
		rep.Body="NO"
	}
	return &rep,nil
}
func (server *CenterServer)handleReg(peopleid string)(*Response,error){//reg frequent user
	YUserName := server.getUserName("Y")
	_,err:=server.DBTool.Reg(peopleid,YUserName)
	var rep Response
	rep.Code = "200"
	if err!=nil{
		rep.Body = err.Error()
		log.Println("reg failed")
	}else {
		rep.Body = "success reg username is " + YUserName
		log.Println("reg success with username "+ YUserName)
	}
	return &rep,err
}

func (server *CenterServer)handleGetAllFrequentUserList()(*Response,error){//获取常用会员
	//log.Println("getting frequent user list")
	var rep Response;
	var userlist []User
	var tmp User
	rows,err:= server.DBTool.GetAllFrequentUser()
	if err!=nil{
		log.Println(err.Error())
		return &rep,nil
	}
	for rows.Next(){
		rows.Scan(&tmp.Cardid,&tmp.Peopleid,&tmp.Money)
		userlist = append(userlist,tmp)
	}
	b,err:=json.Marshal(userlist)
	if err!=nil{
		log.Println(err.Error())
		return &rep,nil
	}
	rep.Code = "200"
	rep.Body = string(b[:]);
	return &rep,nil
}
func (server *CenterServer)handleAddMoney(cardid string , money int)(*Response ,error) {
	var rep Response;
	rep.Code = "200";
	rep.Body = "-1";
	_, err := server.DBTool.AddMoney(cardid, money)
	if err != nil {
		return &rep, err
	}
	rep.Body = "1"
	return &rep,err
}
func (server *CenterServer)handleDelUser(id string)(*Response ,error){
	var rep Response
	var err error
	rep.Code = "200";
	rep.Body,err = server.DBTool.Del(id)
	return &rep,err
}
func (server *CenterServer)handleAskTempUser(num,money int)(*Response,error){
	var ans []string
	var resp Response
	for i:=0;i<num;i++{
		username := server.getUserName("T")
		_,err:= server.DBTool.RegTmp(username,money)
		if err!=nil{
			return &resp,err
		}
		ans = append(ans,username)
	}
	resp.Code ="200"
	b,err :=json.Marshal(ans)
	if err!=nil{
		return &resp,err
	}
	resp.Body = string(b[:])
	return &resp,nil
}
func (server *CenterServer)handleGetOut(cardid string ,price int )(*Response ,error){
	var rep Response
	rep.Code = "200"
	money ,err:= server.DBTool.GetMoney(cardid)
	if err!=nil{
		return &rep,err
	}
	if money<price{
		rep.Body = "money is not success "
		return &rep,nil
	}
	_,err =server.DBTool.AddMoney(cardid,-price)
	if err!=nil{
		return &rep,err
	}
	if string(cardid[0])=="T"{
		_,err = server.DBTool.Del(cardid)
		if err!=nil{
			return &rep,err
		}
	}
	rep.Body =  "get out ok"
	return &rep,nil;
}

