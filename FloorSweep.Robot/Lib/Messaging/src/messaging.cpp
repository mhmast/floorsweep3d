#include "messaging.h"
using namespace FloorSweep::Robot;


Messaging::Messaging(uint8_t pinIn,uint8_t pinOut){
    this->_serial = new SoftwareSerial(pinIn,pinOut,false);
}

Messaging::~Messaging(){
    delete(this->_serial);
    this->_serial = nullptr;
}

void Messaging::begin(){

    this->_serial->begin(MSG_BAUDRATE);
}

bool Messaging::available()
{
    return this->_serial->available();
}

void Messaging::send(const char* message){
    this->_serial->print(message);
}

void Messaging::send(int message){
    this->_serial->print(message);
}
void Messaging::send(const Printable& message){
    this->_serial->print(message);
}

void Messaging::sendln(const char* message){
    this->_serial->println(message);
}
void Messaging::sendln(int message){
    this->_serial->println(message);
}
void Messaging::sendln(const Printable& message){
    this->_serial->println(message);
}

String Messaging::getMessage(){
    
    if(!available()){
      return String();  
    }
    String returnValue =String();
    while(available())
    {
        returnValue += this->_serial->read();
    }
    this->_serial->println(returnValue);
    return returnValue;
    }