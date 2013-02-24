(* This exercises all significant Modula-2 test programs. *)
(* and things like comments of odd and even length *) (***) (****) (*****)

MODULE all ;

(* _______________________________________________________________ *)
(* EXPRESS EXPRESS EXPRESS EXPRESS EXPRESS EXPRESS EXPRESS EXPRESS *)

PROCEDURE express ;

CONST
	little = 3;
	hi = 7;
	middle = 7;

VAR
   good, _bad, ugly : INTEGER;

VAR k, m : INTEGER ;

BEGIN
    good := 42+7-31+1 - 14;

    WRSTR ("Testing expressions:");
    WRLN;
    WRSTR ("42+7-31+1:           should be 19:          ");
    WRINT (42+7-31+1);
    WRLN;
    WRSTR ("good * little:       should be 15:          ");
    WRINT (good * little);
    WRLN;
    WRSTR ("-good * little:      should be -15:         ");
    WRINT (-good * little);
    WRLN;
    WRSTR ("42000 DIV 4200:      should be 10:          ");
    WRINT (42000 DIV 4200);
    WRLN;
    WRSTR ("42042 MOD 100:       should be 42:          ");
    WRINT (42042 MOD 100);
    WRLN;
    WRSTR ("20000 * 21000:       should be 420,000,000: ");
    WRINT (20000 * 21000);
    WRLN;
    WRSTR ("20000 * -21000:     should be -420,000,000: ");
    WRINT (20000 * -21000);
    WRLN;
    WRSTR ("6*7+2*little-1:      should be 47:          ");
    WRINT (6*7+2*little-1);
    WRLN;
    WRSTR ("6*2+((1+2)*3-1)*2:   should be 28:          ");
    WRINT (6*2+((1+2)*3-1)*2);
    WRLN;
    WRLN;

END express ;


(* ___________________________________________________________ *)
(* LOOPANDIF LOOPANDIF LOOPANDIF LOOPANDIF LOOPANDIF LOOPANDIF *)

PROCEDURE loop_and_if ;

CONST
	little = 3;
	hi = 7;
	middle = 7;

VAR
   good, bad, ugly : INTEGER;

VAR k, m : INTEGER ;

BEGIN

    good := 42+7-31+1 - 14;

    WRSTR ("IF test(5):          should be  4:          ");
        k := 4 ;
        m := 3 ;
        IF k > m THEN
            WRINT ( k ) ;
            WRLN ;
        ELSE
            WRINT ( m ) ;
            WRLN ;
        END ;

    WRSTR ("IF test(5.1):        should be 17:          ");
        k := 4 ;
        m := 17 ;
        IF k > m THEN
            WRINT ( k ) ;
            WRLN ;
        ELSE
            WRINT ( m ) ;
            WRLN ;
        END ;

    WRSTR ("IF test(5.2):        should be 71:          ");
        k := 4 ;
        m := 71 ;
        IF k > m THEN
            WRINT ( k ) ;
            WRLN ;
        ELSE

            IF ( m <= 71 ) THEN
                WRINT ( m ) ;
                WRLN ;
            ELSE
                WRINT ( m * 10000 ) ;
                WRLN ;
            END ;

        END ;

    WRSTR ("AND test(5.3):       should be 21:          ");
        k := 4 ;
        m := 71 ;
        IF (k < m) AND ( (k*m) >= (k*m-1) ) THEN
            WRINT ( 21 ) ;
            WRLN ;
        ELSE

            IF m <= 71 THEN
                WRINT ( m ) ;
                WRLN ;
            ELSE
                WRINT ( m * 10000 ) ;
                WRLN ;
            END ;

        END ;

    WRSTR ("AND test(5.4):       should be 22:          ");
        k := 4 ;
        m := 71 ;
        IF (m > k) AND ( (k*m) >= (k*m-1) ) THEN
            WRINT ( 22 ) ;
            WRLN ;
        ELSE

            IF ( m <= 71 ) THEN
                WRINT ( m ) ;
                WRLN ;
            ELSE
                WRINT ( m * 10000 ) ;
                WRLN ;
            END ;

        END ;

    WRSTR ("OR test(5.5):        should be 23:          ");
        k := 4 ;
        m := 71 ;
        IF (k > k) OR (10 <> 10) THEN
            WRINT ( 1111 ) ;
            WRLN ;
        ELSE
            IF ( m <= 71 ) THEN
                WRINT ( 23 ) ;
                WRLN ;
            ELSE
                WRINT ( m * 10000 ) ;
                WRLN ;
            END ;

        END ;

    WRSTR ("OR test(5.6):        should be 24:          ");
    IF (5 > 3) OR ( k*m = (k*m-1) ) THEN
        WRINT ( 24 ) ; WRLN ;  END ;


    WRSTR ("OR AND test(5.7):    should be 25:          ");
    IF (5 < 3) OR ( k*m <> (k*m-1) ) AND (2 = 2) THEN
        WRINT ( 25 ) ; WRLN ;  END ;


    WRSTR ("LOOP test(6):       should be 1-5:        ");
        k := 6 ;
        m := 1 ;
        WRLN ;
        LOOP
            IF m = k THEN EXIT ; END ;
            WRINT ( m ) ;
            WRLN ;
            m := m + 1 ;
        END ;
        WRLN ;


    WRSTR ("LOOP test(6.1):     should be a nice triangle:       ");
        k := 6 ;
        m := 1 ;
        WRLN ;
        LOOP
            IF m = k THEN EXIT ; END ;

            good := 1;
            LOOP
                WRSTR ("@");
                IF good = m THEN EXIT ; END ;
                good := good + 1 ;
            END ;

            WRLN ;
            m := m + 1 ;
        END ;
        WRLN ;

END loop_and_if ;


(* ______________________________________________________________ *)
(* SQUARE SQUARE SQUARE SQUARE SQUARE SQUARE SQUARE SQUARE SQUARE *)

PROCEDURE square ;

TYPE
    prListType = ARRAY [11 .. 30] OF INTEGER ;

VAR
    newSieve
        : prListType ;

    currPrime,
    mult,
    index
        : INTEGER ;

BEGIN

	(* Load values into the array *)
    index := 11 ;
    LOOP
        IF index > 30 THEN EXIT ; END ;
        newSieve[index] := index * index;
        index := index + 1 ;
    END ;

	(* Print the array *)
    index := 11 ;
    LOOP
        IF index > 30 THEN EXIT ; END ;
        WRINT ( index ) ;
        WRSTR ( " squared is " ) ;
        WRINT ( newSieve[index] ) ;
        WRSTR ( "." ) ;
        WRLN ;
        index := index + 1 ;
    END ;

END square ;


(* ___________________________________________________________ *)
(* SIEVE SIEVE SIEVE SIEVE SIEVE SIEVE SIEVE SIEVE SIEVE SIEVE *)

PROCEDURE sieve ;

CONST
    maxIntInSqrt = 8 ;

TYPE
    prListType = ARRAY [2 .. 50] OF INTEGER ;

VAR
    newSieve
        : prListType ;

    currPrime,
    mult,
    index
        : INTEGER ;

BEGIN
    index := 2 ;
    LOOP
        IF index > 50 THEN EXIT END ;
        newSieve[index] := 0 ;
        index := index + 1 ;
    END ;
    currPrime := 2 ;
    LOOP
        IF currPrime > maxIntInSqrt THEN EXIT END ;
        mult := 2 ;
        LOOP
            IF mult * currPrime > 50 THEN EXIT END ;
            newSieve [mult * currPrime] := 1 ;
            mult := mult + 1 ;
        END ;
        LOOP
            currPrime := currPrime + 1 ;
            IF newSieve[currPrime] = 0 THEN EXIT END ;
        END ;
    END ;
    index := 2 ;
    LOOP
        IF index > 50 THEN EXIT END ;
        IF newSieve[index] = 0 THEN
            WRINT ( index ) ;
            WRLN ;
        END ;
        index := index + 1 ;
    END ;

END sieve ;


(* ______________________________________________________________ *)
(* TOWERS TOWERS TOWERS TOWERS TOWERS TOWERS TOWERS TOWERS TOWERS *)

PROCEDURE towersOfHanoi
(* recursively solves the Towers of Hanoi problem;
   Pre:  n is the number of disks;
         i is the start peg;
         j is the goal peg;
   Post: if n = 1 output a move; otherwise decompose the problem into three
         simpler subproblems and solve in sequence:
         1.  move n-1 disks from i to k;
         2.  move 1   disk  from i to j ;
         3.  move n-1 disks from k to j ;
*)

( (*in*)  n, i, j : INTEGER ) ;

VAR
    k                  (* k will be set to the peg # i or j *)
        : INTEGER ;

BEGIN
    IF n = 1 THEN
		WRSTR ("Move ");
        WRINT ( i ) ;
		WRSTR (" to ");
        WRINT ( j ) ;
		WRSTR (".");
        WRLN ;
    ELSE
        k := 6 - i - j ;               (* compute number of third peg *)
        towersOfHanoi ( n-1, i, k ) ;  (* move n-1 disks from i to k  *)
        towersOfHanoi (   1, i, j ) ;  (* move   1 disk  from i to j  *)
        towersOfHanoi ( n-1, k, j ) ;  (* move n-1 disks from k to j  *)
    END ;
END towersOfHanoi ;

(* ______________________________________________________________ *)
(* BUBBLE BUBBLE BUBBLE BUBBLE BUBBLE BUBBLE BUBBLE BUBBLE BUBBLE *)

PROCEDURE bubl ;

	CONST listLength = 11 ;
	TYPE  listType = ARRAY [ 1 .. listLength ] OF INTEGER ;
	VAR   thisList : listType ;

	(****************************************)
	PROCEDURE swap ( VAR a, b : INTEGER ) ;
	VAR temp : INTEGER ;
	BEGIN
		temp := a ;
		a    := b ;
		b    := temp ;
	END swap ;

	(****************************************)
	PROCEDURE random ( VAR seed : INTEGER ) ;
	CONST
		m = 8192 ;
		t = 4361 ;
		c = 3899 ;

	VAR temp : INTEGER ;

	BEGIN
		temp := ( seed * t + c ) MOD m ;
		seed := temp ;
	END random ;

	(*****************************************)
	PROCEDURE fill ( VAR list : listType ) ;
	CONST startSeed = 29 ;
	VAR index, seed : INTEGER ;

	BEGIN
		seed := startSeed ;
		index := 1 ;
		LOOP
			IF index > listLength THEN EXIT END ;

			(* fill with pseudo-random numbers from 1000 to 9191 *)
			list[index] := seed + 1000 ; 
			random ( seed ) ;
			index := index + 1 ;
		END ;
	END fill ;

	(*****************************************)
	PROCEDURE sort ( VAR list : listType ) ;
	VAR index, mark : INTEGER ;

	BEGIN
		LOOP
			mark := 0 ;
			index := 1 ;
			LOOP
				IF index > (listLength - 1) THEN EXIT END ;
				IF list[index] > list[index+1] THEN
					swap ( list[index], list[index+1] ) ;
					mark := 1 ;
				END ;
				index := index + 1 ;
			END ;
			IF mark = 0 THEN EXIT END ;
		END ;
	END sort ;

	(*********************************************)
	PROCEDURE print ( VAR list : listType ) ;
	VAR index : INTEGER ;

	BEGIN
		index := 1 ;
		LOOP
			IF index > listLength THEN EXIT END ;
			WRINT ( list[index] ) ;
			WRLN ;
			index := index + 1 ;
		END ;
	END print ;

BEGIN (* bubl "main" *)
    fill  ( thisList ) ;
    print ( thisList ) ;
    WRLN ;
    sort  ( thisList ) ;
    print ( thisList ) ;
END bubl ;


(* ________________________________________________________________ *)
(* REAL REAL REAL REAL REAL REAL REAL REAL REAL REAL REAL REAL REAL *)

PROCEDURE freal ;

BEGIN (* freal "main" *)
    WRSTR ( "Write the real number 0.035: ") ;
    WRREAL ( 0.035 ) ;
    WRLN ;
    WRSTR ( "Write the real numbers 12.25 and 351.123: ") ;
    WRREAL ( 12.25 ) ;
    WRSTR ( " and " ) ;
    WRREAL ( 351.123 ) ;
    WRLN ;
    WRSTR ( "Their sum is 363.373: " ) ;
    WRREAL ( 351.123 + 12.25 ) ;
    WRLN ;
    WRSTR ( "Their difference is -338,873: " ) ;
    WRREAL ( 12.25 - 351.123 ) ;
    WRLN ;
    WRSTR ( "Their product is 4301.256: " ) ;
    WRREAL ( 351.123 * 12.25 ) ;
    WRLN ;
    WRSTR ( "Their negative product is -4301.256: " ) ;
    WRREAL ( -351.123 * 12.25 ) ;
    WRLN ;
    WRSTR ( "Their quotient (351.123 / 12.25) is 28.663: " ) ;
    WRREAL ( 351.123 / 12.25 ) ;
    WRLN ;
    WRSTR ( "Their reciprocal quotient ( 12.25 / 351.123 ) is 0.035: " ) ;
    WRREAL ( 12.25 / 351.123 ) ;
    WRLN ;
    WRSTR ( "15.3 + 17.113 * 0.125 - 12.1 / 6.05 is 15.439:  " ) ;
    WRREAL ( 15.3 + 17.113 * 0.125 - 12.1 / 6.05 ) ;
    WRLN ;
END freal ;


PROCEDURE fullmenu ;
BEGIN
    WRSTR ("Make a menu selection:"); WRLN;
    WRSTR (" 1. Integer expressions."); WRLN;
    WRSTR (" 2. Relational expressions, IF, LOOP."); WRLN;
    WRSTR (" 3. Squares."); WRLN;
    WRSTR (" 4. Sieve of Eratosthenes."); WRLN;
    WRSTR (" 5. Towers of Hanoi."); WRLN;
    WRSTR (" 6. Bubble sort."); WRLN;
    WRSTR (" 7. Real number expressions."); WRLN;
    WRSTR (" 8. Quit."); WRLN; WRLN;
    WRSTR ("Your selection please: "); 
END fullmenu ;

PROCEDURE shortmenu ;
BEGIN
    WRSTR (" 1 expr;");
    WRSTR (" 2 rel_op;");
    WRSTR (" 3 sq;");
    WRSTR (" 4 sieve;");
    WRSTR (" 5 tower;");
    WRSTR (" 6 bubl;");
    WRSTR (" 7 real;");
    WRSTR (" 8 quit");
    WRSTR (" : "); 
END shortmenu ;

VAR menu_choice : INTEGER ; (* receive menu selection *)

BEGIN (* main *)
    WRSTR ("This program tests all significant Modula-2 features. "); WRLN;
    fullmenu; (* display fully described choices*)

    LOOP 
        menu_choice := RDINT (); WRLN;

        IF menu_choice = 1 THEN express;              END ;
        IF menu_choice = 2 THEN loop_and_if;          END ;
        IF menu_choice = 3 THEN square;               END ;
        IF menu_choice = 4 THEN sieve;                END ;
        IF menu_choice = 5 THEN towersOfHanoi(4,1,3); END ;
        IF menu_choice = 6 THEN bubl;                 END ;
        IF menu_choice = 7 THEN freal;                END ;

        IF menu_choice > 7 THEN EXIT END ;

        shortmenu; (* display reminder*)
    END ;
END all .
