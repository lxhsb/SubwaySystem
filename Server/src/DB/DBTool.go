package DB

import (
	_ "github.com/go-sql-driver/mysql"
	"database/sql"
	"sync"
)

type DBTool struct {
	getPass *sql.Stmt
	reg *sql.Stmt
	getAllFrequentUser *sql.Stmt
	db *sql.DB
	lock *sync.RWMutex
}
func NewDBTool(user,pass,ip,port,name string)(*DBTool, error){
	ans :=&DBTool{}
	ans.lock = &sync.RWMutex{}
	db,err:=sql.Open("mysql",user+":"+pass+"@tcp("+ip+":"+port+")/"+name+"?charset=utf8")
	db.Query("");
	if err!=nil{
		return ans,err
	}
	db.SetMaxIdleConns(10)
	db.SetMaxOpenConns(20)
	ans.getPass,err = db.Prepare("select password from admin_user where username = (?);")
	ans.reg,err = db.Prepare("insert into user values(?,?,0);")
	ans.getAllFrequentUser,err = db.Prepare("select * from user ;")
	ans.db = db
	ans.db.Ping()
	return  ans ,err
}
func (db *DBTool)Close(){
	db.reg.Close()
	db.getPass.Close()
	db.db.Close()
}
func (db *DBTool)GetPass(user string)(string ,error){
	var pass string
	db.lock.RLock()
	defer db.lock.RUnlock()
	rows ,err := db.getPass.Query(user)
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
func (db* DBTool)Reg(peopleid ,username string)(string,sql.Result,error){
	res ,err :=db.reg.Exec(username,peopleid)
	return username,res,err
}
func (db *DBTool)GetAllFrequentUser()(*sql.Rows,error){
	return db.getAllFrequentUser.Query()
}




