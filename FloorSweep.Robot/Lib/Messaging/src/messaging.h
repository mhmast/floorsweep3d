#ifndef __messaging_h
#define __messaging_h
#include <SoftwareSerial.h>
#include <Print.h>
namespace FloorSweep {
    namespace Robot{

class SPISlave{
private: 

char buf [100];
volatile byte pos;
volatile bool process_it;

public:

SPISlave();
~SPISlave();

void begin();

bool available();

void send(const char* message);

void send(int message);
void send(const Printable& message);

void sendln(const char* message);
void sendln(int message);
void sendln(const Printable& message);
String getMessage();

};
    }}
#endif
