#ifndef __messaging_h
#define __messaging_h
#include "Arduino.h"

namespace FloorSweep {
    namespace Robot{
#define MSG_BAUDRATE 9600
class Messaging{

public:

static inline void begin(){
    Serial.begin(MSG_BAUDRATE);
    Serial.println();
}

static inline bool available(){return Serial.available()>0;}


static inline void send(const char* message){
    Serial.println(message);
}
static inline void send(int message){
    Serial.println(message);
}
static inline void send(const Printable& message){
    Serial.println(message);
}

template<typename T>
static inline T getMessage(){
    if(!available()){
      return T();  
    }
    size_t size = sizeof(T);
    int* retVal = malloc(size);
    int* begin = retVal;
    while(retVal - begin != size)
    {
        &retVal = Serial.read();
        retVal += sizeof(int);
    }
    return (T)&begin;
}
};
    }
}
#endif
