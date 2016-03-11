import socket
import time
import sys
def enviar_para_unity (msg):
	TCP_IP = '127.0.0.1'
	TCP_PORT = 12598 #porta do servidor Unity
	BUFFER_SIZE = 1024
	s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
	s.connect((TCP_IP, TCP_PORT))
	s.send(msg)
	data = s.recv(BUFFER_SIZE)
	s.close()
	print "mensagem:", data
	
while(1):
	enviar_para_unity("acelerar;\n")
	time.sleep(2)
	enviar_para_unity("direcionar(70);\n")
	time.sleep(2)
	enviar_para_unity("direcionar(90);\n")
	time.sleep(2)
	enviar_para_unity("direcionar(110);\n")
	time.sleep(10)
	enviar_para_unity("direcionar(90);\n")
	time.sleep(2)
	enviar_para_unity("frear;\n")
	time.sleep(5)
