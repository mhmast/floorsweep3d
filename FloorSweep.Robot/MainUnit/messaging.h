#ifndef __messaging_h
#define __messaging_h
#include "Arduino.h"

namespace FloorSweep::Robot{
#define MSG_BAUDRATE 9600
class Messaging{

public:

static inline void begin(){
    Serial::begin(MSG_BAUDRATE);
}

static inline bool available(){return Serial.available()>0;}

template<T>
static inline void send(T obj){
    Serial::write(obj,sizeof(T));
}

template<T>
static inline T getMessage(){
    if(!available()){
      return default;  
    }
    size_t size = sizeof(T)
    int* retVal = malloc(size);
    int* begin = retVal;
    while(retVal - begin != size)
    {
        &retVal = Serial.read(retval,size);
        retval += sizeof(int);
    }
    return (T)&begin;
}
};
}
#endif