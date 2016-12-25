package Servers

//定义server request response
type Request struct {
	Method string
	Params string
}
type Response struct {
	Code string
	Body string
}
type Server interface {
	Handle(method ,params string) (*Response,error)
}
