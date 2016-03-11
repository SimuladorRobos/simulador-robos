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
    print "mensagem recebida:", data

while(1):
    enviar_para_unity("paraFrente;\n")
    time.sleep(2)
    enviar_para_unity("parar;\n")
    time.sleep(2)
    enviar_para_unity("paraTras;\n")
    time.sleep(2)
    enviar_para_unity("parar;\n")
    time.sleep(2)
