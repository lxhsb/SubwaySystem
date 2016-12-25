package main

import (
	"DBOP"
	"log"
	"os"
	"Servers"
	"net"
	"strconv"
)
var dbtool *DBOP.DBTool
var err error
var ipcserver *Servers.IpcServer
var port int = 1208//监听端口
func main() {
	Init()
	defer dbtool.Close()
	netListener ,err := net.Listen("tcp",":"+strconv.Itoa(port))
	if(err!=nil){
		log.Println(err)
		return
	}
	defer netListener.Close()
	log.Println("waiting for connect")
	for{
		conn,err:=netListener.Accept()
		if err!=nil{
			log.Println("connect error")
			continue
		}
		log.Println(conn.RemoteAddr().String()+" connect success")
		go ipcserver.ReceiveMessage(conn)
	}
}
func Init(){
	/*
	初始化数据库连接
	 */
	dbtool,err = DBOP.NewDBTool("root","root","127.0.0.1","3306","subwaysys")
	//defer  dbtool.Close()
	if err!=nil{
		log.Println(err)
		os.Exit(1)
	}
	var centerserver Servers.Server = Servers.NewCenterServer(dbtool)
	ipcserver = Servers.NewIpcserver(&centerserver)
}
