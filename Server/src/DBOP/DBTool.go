package DBOP

import (
	_ "github.com/go-sql-driver/mysql"
	"database/sql"
	"sync"
)
type DBTool struct {
	getPass *sql.Stmt
	reg *sql.Stmt//for frequent user
	getAllFrequentUser *sql.Stmt
	addMoney *sql.Stmt
	delUser *sql.Stmt
	regTMP *sql.Stmt// reg for temp user
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
	if err!=nil{
		return ans,err
	}
	ans.reg,err = db.Prepare("insert into user values(?,?,0);")
	if err!=nil{
		return ans,err
	}
	ans.getAllFrequentUser,err = db.Prepare("select * from user ;")
	if err!=nil{
		return ans,err
	}
	ans.addMoney,err = db.Prepare("update user  set money = money + (?) where cardid like (?);")
	if err!=nil{
		return ans,err
	}
	ans.delUser ,err = db.Prepare("delete from user where cardid like (?);")
	if err!=nil{
		return ans,err
	}
	ans.regTMP,err = db.Prepare("insert into temp_user value((?),(?));")
	if err!=nil{
		return ans,err
	}
	ans.db = db
	ans.db.Ping()
	
	ans.init()//仅供测试使用  注意删除！！！！
	return  ans ,err
}
func (db *DBTool)Close(){
	defer  db.getPass.Close()
	defer  db.reg.Close()
	defer  db.getAllFrequentUser.Close()
	defer  db.addMoney.Close()
	defer  db.delUser.Close()
	defer  db.regTMP.Close()
	defer  db.db.Close()
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
func (db* DBTool)Reg(peopleid ,username string)(string,error){
	_ ,err :=db.reg.Exec(username,peopleid)
	if err!=nil{
		return  "NO",err
	}
	return  "YES",nil
}
func (db *DBTool)GetAllFrequentUser()(*sql.Rows,error){

	return db.getAllFrequentUser.Query()
}
func (db *DBTool)AddMoney(id string ,money int)(string ,error){
	_,err :=db.addMoney.Exec(money,id)
	if err!=nil{
		return  "NO",err
	}
	return  "YES",nil
}
func (db *DBTool)Del(id string )(string,error){
	_,err := db.delUser.Exec(id)
	if err!=nil{
		return "NO",err
	}
	return  "YES",nil
}
func (db *DBTool)RegTmp(cardid string ,money int )(string ,error){
	_,err := db.regTMP.Exec(cardid,money)
	if err!=nil{
		return  "NO",err
	}
	return  "YES",nil
}
func (db *DBTool)init(){
	db.db.Exec("delete  from user");
	db.db.Exec("delete from temp_user");
}
