#include <ESP8266WiFi.h>       
#include "messaging.h"
using namespace FloorSweep::Robot;

const char* ssid     = "Draadloos";         // The SSID (name) of the Wi-Fi network you want to connect to
const char* password = "09051988AA";     // The password of the Wi-Fi network

void setup() {
  
  Messaging::begin();         // Start the Serial communication to send messages to the computer
  delay(10);
  
  WiFi.begin(ssid, password);             // Connect to the network
  Messaging::send("Connecting to ");
  Messaging::send(ssid); 
  Messaging::send(" ...");

  int i = 0;
  while (WiFi.status() != WL_CONNECTED) { // Wait for the Wi-Fi to connect
    delay(1000);
    Messaging::send(++i); 
    Messaging::send(" ");
  }

  Messaging::send("\n");
  Messaging::send("Connection established!");  
  Messaging::send("IP address:\t");
  Messaging::send(WiFi.localIP());         // Send the IP address of the ESP8266 to the computer
}

void loop() { }
