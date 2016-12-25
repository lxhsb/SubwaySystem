package Servers

type Account struct {// use for admin login
	User string
	Pass string
}
type User struct {//real user
	Cardid string
	Peopleid string
	Money int
}
type AddMoneyOP struct { //加钱操作
	Cardid string
	Money int

}
type AskTempRegOP struct{ //临时注册操作
	Num int
	Money int

}