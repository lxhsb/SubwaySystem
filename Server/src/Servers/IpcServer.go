package Servers

import (
	"net"
	"log"
	"encoding/json"
)

//定义ipcserver 主要负责与客户端之间的通信
//receive并解码一个request 传递给handle来实现 再编码 通过send来发送给客户端
type IpcServer struct {
	CenterServer Server
}
func NewIpcserver (server *Server) *IpcServer{
	return &IpcServer{CenterServer:*server}
}
func (ipcserver *IpcServer)ReceiveMessage(conn net.Conn){//这点可能会出bug 待修改

	buf := make([]byte ,1<<21)
	for{
		len,err:=conn.Read(buf)
		if err!=nil{
			log.Println("err receive from" + conn.RemoteAddr().String()+" maybe disconnected")
			break
		}
		var req Request
		err=json.Unmarshal(buf[0:len],&req)

		if err!=nil{//这点先这样写
			log.Println("err unmarshal json from " + conn.RemoteAddr().String())
			continue
		}
		rep,err:= ipcserver.CenterServer.Handle(req.Method,req.Params)
		ipcserver.send(*rep,conn)
	}
}
func (ipcserver *IpcServer)send(rep Response,conn net.Conn)(error){
	b,err:= json.Marshal(rep)
	if err!=nil{
		log.Println("err in send ,try to send to "+conn.RemoteAddr().String())
	}
	conn.Write(b)
	return nil
}