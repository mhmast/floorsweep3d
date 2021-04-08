#include "LCD.h"
#include <SR04.h>

#define NOCOLUMNS 16
#define NOROWS 2
#define TRIGGERPIN 5
#define ECHOPIN 6

SR04 hc(ECHOPIN, TRIGGERPIN);
LCD lcd(7, 8, 9, 10, 11, 12);

void setup()
{
  lcd.init(NOCOLUMNS, NOROWS);
  
  Serial.begin(9600);
}




void loop()
{
    long distance = hc.Distance();
  String text = getText("Distance : {0} cm",distance);
  lcd.setDisplayText(text);

  
  //delay(500);
}


const String getText(const char* a,long arg1)
{
  String tmp = String(a);
  tmp.replace(String("{0}"),String("")+arg1);
  return tmp;
}
