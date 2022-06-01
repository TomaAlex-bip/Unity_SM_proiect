from imu import MPU6050
from machine import UART, Pin, I2C
import time

led = Pin(25, Pin.OUT)
ser = UART(1, 9600)  # tx=6 rx=7
i2c = I2C(0, sda=Pin(16), scl=Pin(17), freq=400000)
imu = MPU6050(i2c)

# Pin constants
PIN27 = 21  # GP21
PIN26 = 20  # GP20
PIN24 = 18  # GP18

# Led connections
led_rosu = Pin(PIN27, Pin.OUT)
led_galben = Pin(PIN26, Pin.OUT)
led_verde = Pin(PIN24, Pin.OUT)

led.high()

# initial oprite
led_rosu.value(1)
led_galben.value(1)
led_verde.value(1)

while 1:
    
    ax=round(imu.accel.x,2)
    ay=round(imu.accel.y,2)
    az=round(imu.accel.z,2)
    gx=round(imu.gyro.x)
    gy=round(imu.gyro.y)
    gz=round(imu.gyro.z)
    tem=round(imu.temperature,2)
    
    axaX = ax * az
    axaZ = ax * ay
    #print(axaX, "\t", axaZ, "\t        ", end="\r")
        
    time.sleep(0.1)
    
    ser.write(str(axaX) + "~" + str(axaZ) + "\r\n");
    
    c = ser.read(1024)
            
    if c == b'over':
        #print(c)
        led_rosu.value(0)  # se aprinde led rosu
        time.sleep(3)  # 3 secunde pentru game over
        led_rosu.value(1)  # se stinge led rosu
    
    if c == b'coin':
        #print(c)
        led_galben.value(0)  # se aprinde led galben
        time.sleep(1)  # 1 secudna pentru led galben aprins
        led_galben.value(1)  # se stinge led galben
        
    if c == b'win':
        #print(c)
        led_verde.value(0)  # se aprinde led verde
        time.sleep(3)  # 3 secunde pentru victorie
        led_verde.value(1)  # se stinge led verde
        
