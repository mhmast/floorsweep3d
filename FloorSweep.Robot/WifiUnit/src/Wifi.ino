#include <ESP8266WiFi.h>       
#include <messaging.h>
using namespace FloorSweep::Robot;

const char* ssid     = "Draadloos";         // The SSID (name) of the Wi-Fi network you want to connect to
const char* password = "09051988AA";     // The password of the Wi-Fi network
Messaging messaging(6,5);

void setup() {
  
  messaging.begin();         // Start the Serial communication to send messages to the computer
  delay(10);
  
  WiFi.begin(ssid, password);             // Connect to the network
  messaging.send("Connecting to ");
  messaging.send(ssid); 
  messaging.sendln(" ...");

  int i = 0;
  while (WiFi.status() != WL_CONNECTED) { // Wait for the Wi-Fi to connect
    delay(1000);
    messaging.send(++i); 
    messaging.sendln(" ");
  }

  messaging.sendln("Connection established!");  
  messaging.send("IP address: ");
  messaging.sendln(WiFi.localIP());         // Send the IP address of the ESP8266 to the computer
}

void loop() {
messaging.sendln("hello");

 }
