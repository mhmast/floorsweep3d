
#include "LCD.h"


uint8_t NOCOLUMNS ;
uint8_t NOROWS ;
uint8_t TOTALCHARLEN;
uint8_t index = 0;
String displayText;

LCD::LCD(uint8_t rs, uint8_t enable,		uint8_t d0, uint8_t d1, uint8_t d2, uint8_t d3)
        {
            liquidCrystal = new LiquidCrystal(rs,enable,d0,d1,d2,d3);
        }
LCD::~LCD(){
    delete liquidCrystal;
    liquidCrystal = NULL;
}
void LCD::init(uint8_t no_rows,uint8_t no_columns)
{
    NOCOLUMNS = no_columns;
    NOROWS = no_rows;
    TOTALCHARLEN = NOCOLUMNS;
     liquidCrystal->begin(NOCOLUMNS, NOROWS);
  for (uint8_t i = 0; i < TOTALCHARLEN; i++)
  {
    displayText = displayText + ' ';
  }
}

void DoNewsPaper(String text)
{
  const uint8_t upper = text.length() - index;
  for (uint8_t i = 0; i < TOTALCHARLEN; i++)
  {
    if (i < index)
    {
      displayText.setCharAt(i, text.charAt(text.length() - index + i));
    }
    else if (i >= index && i - index < text.length() - 1)
    {
      displayText.setCharAt(i, text.charAt(i - index));
    }
    else
    {
      displayText.setCharAt(i, ' ');
    }
  }

  index++;
  if (index > text.length())
  {
    index = 0;
  }

}

void LCD::setDisplayText(String text,bool isNewspaper )
{
    if(!isNewspaper)
    {
displayText = text;
  
    }
  else{
      DoNewsPaper(text);
      }

      liquidCrystal->setCursor(0, 0);
  liquidCrystal->write(displayText.c_str());
  if (displayText.length() > TOTALCHARLEN)
  {
    liquidCrystal->setCursor(0, 1);
    liquidCrystal->write(displayText.substring(TOTALCHARLEN).c_str());
  }
}
