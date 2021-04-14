#ifndef LCD_h
#define LCD_h

#include <LiquidCrystal.h>

class LCD 
{
public:
 
LCD(uint8_t rs, uint8_t enable,uint8_t d0, uint8_t d1, uint8_t d2, uint8_t d3);
~LCD();
void init(uint8_t no_rows,uint8_t no_columns);
void setDisplayText(String text,bool isNewspaper = false);

private: 
LiquidCrystal* liquidCrystal;

};

#endif