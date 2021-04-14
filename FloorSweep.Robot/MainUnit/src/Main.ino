#include <LCD.h>
#include <messaging.h>
#include <SR04.h>

#define NOCOLUMNS 16
#define NOROWS 2
#define TRIGGERPIN 5
#define ECHOPIN 6
using namespace FloorSweep::Robot;
SR04 hc(ECHOPIN, TRIGGERPIN);
LCD lcd(7, 8, 9, 10, 11, 12);
Messaging messaging(A0,A1);

void setup()
{
  messaging.begin();
  lcd.init(NOROWS,NOCOLUMNS);
}
void loop()
{
  //   long distance = hc.Distance();
  // String text = getText("Distance : {0} cm",distance);
  lcd.setDisplayText(String("Awaiting message...")+String(messaging.available()?"true":"false"));
delay(1000);
  if(messaging.available())
  {
    lcd.setDisplayText(String("Message received"));
    String text = messaging.getMessage();
    delay(1000);
    lcd.setDisplayText(text);
  }
  
  //delay(500);
}


const String getText(const char* a,long arg1)
{
  String tmp = String(a);
  tmp.replace(String("{0}"),String("")+arg1);
  return tmp;
}
