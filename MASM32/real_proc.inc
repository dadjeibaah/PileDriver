;============== BEGIN PROCEDURE ============
; PRE: PutBits accepts a single real number (32-bit formatted per the IEEE
;    Standard 754 for Binary Floating-Point Arithmetic) which has been 
;    stored in EAX.
; POST: The number is displayed on the screen as a binary representation of
;    the 32-bits of the IEEE format.

PutBits PROC  ; Procedure definition
        push    BP    ; save BP since we use it 
        mov     BP,SP ;
        push    EAX   ; save the registers we use
        push    EBX   ; save the registers we use
        push    ECX   ; save the registers we use
        PutStr  rstr2      ; marker strip
        nwln        
        mov     ecx,0 ; counter
binloop:
        mov     EBX,EAX
        and     EBX,80000000h ; mask off all but msb
        mov     DL, 30h ; "0" assumed
        cmp     EBX,0
        je      zeroOut
        mov     DL, 31h ; "1"
zeroOut:
        push    EAX ; store EAX while using int 21h command
        mov     AH,02h; print character to STDOUT
        int     21h
        add     ECX,1 ;
        cmp     ECX,32 ;
        je      done
        pop     EAX
        shl     EAX,1 ; shift logical left 1 bit 
        jmp     binloop
done:
        mov     AH,02h; print character to STDOUT
        mov     DL,20h; " "
        int     21h
        mov     DL,68h; "h"
        int     21h
        nwln
        pop     ECX   ; restore the registers we use
        pop     EBX   ; restore the registers we use
        pop     EAX   ; restore the registers we use
        pop     BP    ;  
        ret     0 ;  No bytes were allocated by this program.
PutBits ENDP
;=============== END PROCEDURE =============

;============== BEGIN PROCEDURE ============
; PRE: PutFloat accepts a single real number (32-bit formatted per the IEEE
;    Standard 754 for Binary Floating-Point Arithmetic) which has been 
;    stored in EAX.
; POST: The number is displayed on the screen as a decimal number with three
;    decimal places. The last bit is rounded.

PutFloat PROC  ; Procedure definition
        push    BP    ; save BP since we use it 
        push    EAX   ; save the registers we use
        push    EBX   ; save the registers we use
        push    ECX   ; save the registers we use
        push    EDX   ; save the registers we use
        mov     BP,SP ;

        mov     EBX, EAX ; prepare to test the sign bit
        and     EBX,80000000h ; mask off all but msb which is sign bit
        mov     DL, 20h ; " " assumed
        cmp     EBX,0
        je      spaceOut
        mov     DL, 2dh ; "-"
spaceOut:
        push    EAX ; store EAX while using int 21h command
        mov     AH,02h; print character to STDOUT
        int     21h   ; print sign or space
        pop     EAX   ; restore the REAL to EAX
        mov     EBX, EAX ; prepare to extract the exponent bits
        shl     EBX, 1  ;  remove the sign bit
        shr     EBX, 24 ; remove significand bits
        sub     EBX, 127 ; subtract bias
        cmp     EBX, 0  ; test for zero integer part. Is the exponent (EBX) < 0?
        jl      zeroInteger

nonZeroInteger:        
        push    EBX ; store exponent
        mov     EBX, EAX ; prepare to extract the integer portion
        shl     EBX, 9  ;  remove the sign bit and exponent bits
        shr     EBX, 1  ;  prepare to add implied 1 bit
        or      EBX, 80000000h ;  add the implied 1 bit
        pop     ECX  ; load the exponent for the coming subtraction
        push    ECX  ; re-store the exponent; we need it later
        mov     EDX, 31  ; bits to remove if exponent is zero
        sub     EDX, ECX
        mov     ECX, EDX
        shr     EBX, CL ; shift right by this number of bits 
        PutLint EBX
        jmp     printDotIntAtLeastOne
        
zeroInteger:
        neg     EBX ; make the exponent positive
        push    EBX ; store exponent
        mov     EBX, 0
        PutLint EBX     ; print the 0 for a REAL less than 1        
        push    EAX ; store EAX while using int 21h command
        mov     DL, 2eh ; "."
        mov     AH, 02h; print character to STDOUT
        int     21h   ; print decimal point
        pop     EAX
        mov     EBX, EAX ; prepare to extract the fraction portion
                         ; assume the fraction is x/2048 so we want 11 bits
        shl     EBX, 9  ; remove the sign bit and exponent bits
        shr     EBX, 1  ;  prepare to add implied 1 bit
        or      EBX, 80000000h ;  add the implied 1 bit
        shr     EBX, 21 ; remove all but 11 bits
        pop     ECX     ; retrieve the now positive exponent
        dec     ECX     ; we will shift a bit less (due to the implicit 1)
        shr     EBX, CL ; shift by this number of bits
        jmp     printFraction
        
printDotIntAtLeastOne:        
        push    EAX ; store EAX while using int 21h command
        mov     DL, 2eh ; "."
        mov     AH, 02h; print character to STDOUT
        int     21h   ; print decimal point
        pop     EAX
        mov     EBX, EAX ; prepare to extract the fraction portion
                         ; assume the fraction is x/2048 so we want 11 bits
        shl     EBX, 9  ; remove the sign bit and exponent bits
        pop     ECX     ; load the exponent again
        shl     EBX, CL ; remove integer bits
        shr     EBX, 21 ; remove all but 11 bits

printFraction:
        inc     EBX     ; add one bit to simulate rounding least significant bit
        mov     EAX, 1000 ; multiply by 1000
        imul    BX     ; numerator times 1000 - this leaves the bits in the right place for idiv
        mov     EBX, 2048
        idiv    BX       ; divide by 2048, 
                         ;    then print with right number of leading zeros
        cwde             ; sign-extend AX into EAX to clean out upper 16 bits of EAX
        cmp     EAX,0    ;
        jne     oneDigit
        PutStr  rstrThree0
        jmp     DonePutFloat
oneDigit:                ; e.g. .007
        cmp     EAX,10   ;
        jge     twoDigits
        PutStr  rstrTwo0
        PutLint EAX
        jmp     DonePutFloat
twoDigits:                ; e.g. .035
        cmp     EAX,100   ;
        jge     threeDigits
        PutStr  rstrOne0
        PutLint EAX
        jmp     DonePutFloat
threeDigits:              ; e.g. .420
        PutLint EAX

DonePutFloat:     
        pop     EDX   ; restore the registers we use
        pop     ECX   ; restore the registers we use
        pop     EBX   ; restore the registers we use
        pop     EAX   ; restore the registers we use
        pop     BP    ;  
        ret     0     ; No bytes were allocated by this procedure.
PutFloat ENDP
;=============== END PROCEDURE =============

