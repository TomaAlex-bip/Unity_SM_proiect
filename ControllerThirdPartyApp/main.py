import socket
import threading
import time

localhost = "127.0.0.1"
mac_masinarie = "98:d3:31:fd:80:db"
channel = 1


class Server:
    def __init__(self, port):
        self.port = port
        self.socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        self.bt_socket = socket.socket(socket.AF_BLUETOOTH, socket.SOCK_STREAM, socket.BTPROTO_RFCOMM)

        self.thread1 = threading.Thread(target=self.send_from_unity_to_jhonny)
        self.thread2 = threading.Thread(target=self.send_from_jhonny_to_unity)

    def start_server(self):
        self.socket.bind((localhost, self.port))
        self.socket.listen()
        self.conn, addr = self.socket.accept()
        print(f"connected to: {addr}")

        self.bt_socket.connect((mac_masinarie, channel))
        print(f"connected to: {mac_masinarie}")

        self.thread1.start()
        self.thread2.start()


    def send_from_unity_to_jhonny(self):
        while True:
            msg = self.conn.recv(1024)
            if len(msg) <= 0:
                continue

            try:
                self.bt_socket.send(msg)
            except OSError:
                pass

    def send_from_jhonny_to_unity(self):
        while True:
            # msg = self.bt_socket.makefile().readline()
            msg = self.bt_socket.recv(1024)
            if len(msg) <= 0:
                continue

            try:
                print(msg)
                # self.conn.send(bytes(msg.encode('utf-8')))
                self.conn.send(msg)
            except OSError:
                pass


def main():
    server = Server(50000)
    server.start_server()


if __name__ == "__main__":
    main()





 # self.bt_socket.send(bytes(msg.encode('utf-8')))
        # conn.sendall(bytes("date importante".encode('utf-8')))
        # print(f"am trimis date catre {addr}")
        # time.sleep(1)