package main

import (
	"net"
	"strconv"
	"log"
	"ipc"
	"DB"
	"os"
	//"time"
)



var port int = 1208
var ip string = "127.0.0.1"

func main(){

	dbtool ,err := DB.NewDBTool("root","root","127.0.0.1","3306","subwaysys")
	defer dbtool.Db.Close()//本来不行把这个给开放的 但是如果不开放的话 根本没法关闭数据库连接
	//***要多学习设计模式***
	if err!=nil{
		log.Println(err)
		os.Exit(1)
	}
	server := ipc.NewIpcServer("fuck",dbtool)
	netListener ,err := net.Listen("tcp",ip+":"+strconv.Itoa(port))
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
		go server.ReceiveMessage(conn)
	}

}
