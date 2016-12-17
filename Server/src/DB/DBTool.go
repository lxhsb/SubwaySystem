package DB

import (
	_"github.com/go-sql-driver/mysql"
	"database/sql"
	"time"
	"strconv"
	"sync"
)

type DBTool struct {
	Db *sql.DB
	lock *sync.RWMutex
}
func NewDBTool(user,pass,ip,port,name string)(*DBTool, error){
	ans :=&DBTool{}
	ans.lock = &sync.RWMutex{}
	db,err:=sql.Open("mysql",user+":"+pass+"@tcp("+ip+":"+port+")/"+name+"?charset=utf8")
	if err!=nil{
		return ans,err
	}
	//defer db.Close()
	ans.Db = db
	_ ,err = db.Exec("use "+name+";")//go 语言默认不是open就连接，这里这样搞一下是为了防止用户自真正使用的时候 出现第一下非常慢的情况
	return  ans ,err
}

func (db *DBTool)GetPass(user string)(string ,error){
	var pass string
	db.lock.RLock()
	defer db.lock.RUnlock()
	rows ,err := db.Db.Query("select password from admin_user where username = (?);",user)
	if err!=nil{
		return "",err
	}
	for rows.Next(){
		err := rows.Scan(&pass)
		if err!=nil{
			return "",err
		}
		return pass,nil
	}
	return pass,nil

}
func (db* DBTool)Reg(peopleid string)(string,sql.Result,error){
	t := time.Now()//接下来是臭长一段代码 ，好想给他打成md5 但是那样估计会被骂？
	//你问我为啥不直接给个用户名注册？ 你去地铁办卡的时候有用户名吗？？？
	var strTime = strconv.Itoa(t.Year())+t.Month().String()+strconv.Itoa(t.Day())+strconv.Itoa(t.Hour())+strconv.Itoa(t.Minute())+strconv.Itoa(t.Second())+strconv.Itoa(t.Nanosecond())
	var username = "Y"+strTime
	res ,err :=db.Db.Exec("insert into user values(?,?,0);",username,peopleid)
	return username,res,err
}





