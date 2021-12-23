int simpleCalculator(int a, int n, int b) {
    /*
      1  - Addition             2 - Subtraction       3 - Multiply
      4  - Division             5 - Equal             6 - Not equal
      7  - Bitwise OR           8 - Bitwise XOR       9 - Bitwise AND
      10 - Modulo              11 - OR               12 - AND
      13 - Less than           14 - Less than equal  15 - Greater than
      16 - Greater than equal  17 and more - 0
    */

   
    int answer = n == 1 ? a + b :
        n == 2 ? a - b :
        n == 3 ? a * b :
        n == 4 ? a / b :
        n == 5 ? a == b :
        n == 6 ? a != b :
        n == 7 ? a | b :
        n == 8 ? a ^ b :
        n == 9 ? a & b :
        n == 10 ? a % b :
        n == 11 ? a || b :
        n == 12 ? a && b :
        n == 13 ? a < b :
        n == 14 ? a <= b :
        n == 15 ? a > b :
        n == 16 ? a >= b : 0;
    return answer;)
}

int main() {
    return simpleCalculator(30, 2, 24);
}

