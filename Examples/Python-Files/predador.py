import socket
import time
#API basica do para comunicar com o robo predador
def enviar_para_unity (msg):
    TCP_IP = '127.0.0.1'
    TCP_PORT = 12598 #porta do servidor Unity
    BUFFER_SIZE = 1024
    s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    s.connect((TCP_IP, TCP_PORT))
    s.send(msg)
    data = s.recv(BUFFER_SIZE)
    s.close()
    return data
def frente():
    enviar_para_unity("frente;\n")
def direita():
    enviar_para_unity("direita;\n")
def esquerda():
    enviar_para_unity("esquerda;\n")
def tras():
    enviar_para_unity("tras;\n")
def parar():
    enviar_para_unity("parar;\n")
def distanciaE():
    return ord(enviar_para_unity("distanciaE;\n")[0])
def distanciaC():
    return ord(enviar_para_unity("distanciaC;\n")[0])
def distanciaD():
    return ord(enviar_para_unity("distanciaD;\n")[0])
def cor():
    string = enviar_para_unity("cor;\n")
    numero = ord(string[0])*256*256 + ord(string[1])*256 + ord(string[2]) #0xRRGGBB
    return numero
while(1):
    dEsquerda = distanciaE()
    dDireita = distanciaD()
    dCentral = distanciaC()

    print "DistanciaC" + str(dCentral)
    print "DistanciaD" + str(dDireita)
    print "DistanciaE" + str(dEsquerda)
    
    if(dEsquerda < dDireita and dEsquerda < dCentral):
        print 'Indo para esquerda'
        esquerda()
    else:
        if(dCentral < dDireita):
            print 'Indo para frente'
            frente()
        else:
            print 'Indo para direita'
            direita()
    
    

