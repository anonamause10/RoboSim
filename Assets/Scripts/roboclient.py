import socket
client = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
client.connect(('127.0.0.1', 8052))
client.send(str.encode("I am CLIENT"))
while True:
    from_server = str(client.recv(1024))
    from_server = from_server[2:len(from_server)-1]
    if(from_server == "Stop"):
        print('stopping')
        break
    print (from_server)
client.close()