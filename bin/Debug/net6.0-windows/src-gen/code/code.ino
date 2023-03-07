

double B1_3Value ; 
double B2_3Value ; 
double B3_3Value ; 
double temp3;
double B4_3Value(double var1, double var2 ){
	return var1+var2;
}
boolean temp4;
 boolean C1_3Value(double var1, double var2){
	return (var1 < var2);
}
boolean B5_3Value ; 
void setup(){
	Serial.begin(9600);

 pinMode(B2_3Value ,INPUT);
 pinMode(B3_3Value ,INPUT);
 pinMode(13,OUTPUT);} 
void loop(){ 
B1_3Value = 10; 
B2_3Value = 6; 
B3_3Value = 6; 
temp3 = B4_3Value(B2_3Value,B3_3Value);
temp4 = C1_3Value(B1_3Value,temp3);
digitalWrite(13,temp4);
 			}