int sensorPin = A0;    // select the input pin for the potentiometer
int ledPin = 13;      // select the pin for the LED
int sensorValue = 0;  // variable to store the value coming from the sensor

unsigned long startTime = 0;
long heavyAttackTreshold = 1500;
bool buttonPressed = false;
bool heavyLaunched = false;

void setup() {
  // declare the ledPin as an OUTPUT:
  Serial.begin(9600);
  pinMode(ledPin, OUTPUT);
}

void loop() {
  // read the value from the sensor:
  sensorValue = analogRead(sensorPin);
  
  if(sensorValue > 350) {
    if(startTime == 0) {
      startTime = millis();
      buttonPressed = true;
      Serial.print("begin");
      Serial.println();
    }
      
    // If we're over heavy attack treshold
    if(millis() - startTime >= heavyAttackTreshold && !heavyLaunched) {
      // Send heavy attack signal
      Serial.print("heavy");
      Serial.println();
      heavyLaunched = true;
    }

    // turn the ledPin on
    digitalWrite(ledPin, HIGH);  
  } else {
    if(buttonPressed) {
      // if timer is under heavy attack treshold
      if(millis() - startTime < heavyAttackTreshold) {
        // Launch light attack
        Serial.print("light");
        Serial.println();
      }
      
      heavyLaunched = false;
      buttonPressed = false;
    }
    
    startTime = 0;
    
    // turn the ledPin off
    digitalWrite(ledPin, LOW);  
  }
}

