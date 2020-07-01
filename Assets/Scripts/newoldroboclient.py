import socket
import time
SERVER = "127.0.0.1"
PORT = 8052

class Robot:
    def __init__(self):
        self.connected = False 
        self.client = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        self.client.connect((SERVER, PORT))
        string = "connectPlz "
        self.client.sendall(bytes(string,'UTF-8'))
        in_data =  self.client.recv(1024).decode()
        if(in_data == "ok connected"):
            self.connected = True


    
    def setMaxForwardVel(self, val):
        string = "robocommand setMaxForwardVel " + str(val)
        self.client.sendall(bytes(string,'UTF-8'))
        self.waitForDone()

    def setMaxSideVel(self, val):
        string = "robocommand setMaxSideVel " + str(val)
        self.client.sendall(bytes(string,'UTF-8'))
        self.waitForDone()

    def setMaxTurnVel(self, val):
        string = "robocommand setMaxTurnVel " + str(val)
        self.client.sendall(bytes(string,'UTF-8'))
        self.waitForDone()

    def setForwardVel(self, val):
        string = "robocommand setForwardVel " + str(val)
        self.client.sendall(bytes(string,'UTF-8'))
        self.waitForDone()

    def setSideVel(self, val):
        string = "robocommand setSideVel " + str(val)
        self.client.sendall(bytes(string,'UTF-8'))
        self.waitForDone()

    def setTurnVel(self, val):
        string = "robocommand setTurnVel " + str(val)
        self.client.sendall(bytes(string,'UTF-8'))
        self.waitForDone()

    def getMaxForwardVel(self):
        string = "robocommand getMaxForwardVel"
        self.client.sendall(bytes(string,'UTF-8'))
        self.waitForDone()
        in_data =  self.client.recv(1024).decode()
        dataParts = in_data.split(' ')
        if(dataParts[0] != "maxForwardVel"):
            return -1
        return float(dataParts[1])
        

    def getMaxSideVel(self):
        string = "robocommand getMaxSideVel"
        self.client.sendall(bytes(string,'UTF-8'))
        self.waitForDone()
        in_data =  self.client.recv(1024).decode()
        dataParts = in_data.split(' ')
        if(dataParts[0] != "maxSideVel"):
            return -1
        return float(dataParts[1])

    def getMaxTurnVel(self):
        string = "robocommand getMaxTurnVel"
        self.client.sendall(bytes(string,'UTF-8'))
        self.waitForDone()
        in_data =  self.client.recv(1024).decode()
        dataParts = in_data.split(' ')
        if(dataParts[0] != "maxTurnVel"):
            return -1
        return float(dataParts[1])

    def getForwardVel(self):
        string = "robocommand getForwardVel"
        self.client.sendall(bytes(string,'UTF-8'))
        self.waitForDone()
        in_data =  self.client.recv(1024).decode()
        dataParts = in_data.split(' ')
        if(dataParts[0] != "forwardVel"):
            return 0
        return float(dataParts[1])

    def getSideVel(self):
        string = "robocommand getSideVel"
        self.client.sendall(bytes(string,'UTF-8'))
        self.waitForDone()
        in_data =  self.client.recv(1024).decode()
        dataParts = in_data.split(' ')
        if(dataParts[0] != "sideVel"):
            return 0
        return float(dataParts[1])

    def getTurnVel(self):
        string = "robocommand getTurnVel"
        self.client.sendall(bytes(string,'UTF-8'))
        self.waitForDone()
        in_data =  self.client.recv(1024).decode()
        dataParts = in_data.split(' ')
        if(dataParts[0] != "turnVel"):
            return 0
        return float(dataParts[1])

    def getTrueVelocity(self):
        string = "robocommand getTrueVelocity"
        self.client.sendall(bytes(string,'UTF-8'))
        self.waitForDone()
        in_data =  self.client.recv(1024).decode()
        dataParts = in_data.split(' ')
        if(dataParts[0] != "trueVel"):
            return (0,0,0)
        return (float(dataParts[1]),float(dataParts[2]),float(dataParts[3]))

    def getTrueAngularVelocity(self):
        string = "robocommand getTrueAngularVelocity"
        self.client.sendall(bytes(string,'UTF-8'))
        self.waitForDone()
        in_data =  self.client.recv(1024).decode()
        dataParts = in_data.split(' ')
        if(dataParts[0] != "trueAngVel"):
            return 0
        return float(dataParts[1])

    def getGyroAngle(self):
        string = "robocommand getGyroAngle"
        self.client.sendall(bytes(string,'UTF-8'))
        self.waitForDone()
        in_data =  self.client.recv(1024).decode()
        dataParts = in_data.split(' ')
        if(dataParts[0] != "gyroAngle"):
            return 0
        return float(dataParts[1])

    def getForwardDist(self):
        string = "robocommand getForwardDist"
        self.client.sendall(bytes(string,'UTF-8'))
        self.waitForDone()
        in_data =  self.client.recv(1024).decode()
        dataParts = in_data.split(' ')
        if(dataParts[0] != "forwardDist"):
            return 0
        return float(dataParts[1])

    def getBackDist(self):
        string = "robocommand getBackDist"
        self.client.sendall(bytes(string,'UTF-8'))
        self.waitForDone()
        in_data =  self.client.recv(1024).decode()
        dataParts = in_data.split(' ')
        if(dataParts[0] != "backDist"):
            return 0
        return float(dataParts[1])

    def getLeftDist(self):
        string = "robocommand getLeftDist"
        self.client.sendall(bytes(string,'UTF-8'))
        self.waitForDone()
        in_data =  self.client.recv(1024).decode()
        dataParts = in_data.split(' ')
        if(dataParts[0] != "leftDist"):
            return 0
        return float(dataParts[1])

    def getRightDist(self):
        string = "robocommand getRightDist"
        self.client.sendall(bytes(string,'UTF-8'))
        self.waitForDone()
        in_data =  self.client.recv(1024).decode()
        dataParts = in_data.split(' ')
        if(dataParts[0] != "rightDist"):
            return 0
        return float(dataParts[1])

    def waitForDone(self):
        while(not self.client.recv(1024).decode()==" command executed"):
            None 

    def stop(self):
        self.client.close()

robot = Robot()
while(not robot.connected):
    None
robot.setForwardVel(0.5)
time.sleep(3)
robot.setForwardVel(0)
print(robot.getForwardDist())
robot.setTurnVel(1)
while(robot.getGyroAngle()>90):
    None
robot.setTurnVel(0)
robot.stop()