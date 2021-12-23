.386

.model flat, stdcall

option casemap:none

include \masm32\include\masm32rt.inc

.data
a_simpleCalculator dd 0
n_simpleCalculator dd 0
b_simpleCalculator dd 0
answer_simpleCalculator dd 0
solution_simpleCalculator dd 0
solution_main dd 0

.code

start:
main proc
	mov eax,30
	mov a_simpleCalculator, eax
	mov eax,2
	mov n_simpleCalculator, eax
	mov eax,24
	mov b_simpleCalculator,eax 
	call simpleCalculator
	mov eax,solution_simpleCalculator
	mov solution_main,eax
	fn MessageBox,0,str$(solution_main), "Andrii Sliusarenko IO-91", MB_OK
	ret
main endp


simpleCalculator proc
	mov eax,n_simpleCalculator
	mov ebx,1
	cmp eax,ebx
	je @start1
	mov eax,0
	jmp @end1
	@start1:
	mov eax,1
	@end1:
	cmp eax,1
	jne @false4
	mov eax,a_simpleCalculator
	mov ebx,b_simpleCalculator
	add eax,ebx
	jmp @exit
	@false4:
	mov eax,n_simpleCalculator
	mov ebx,2
	cmp eax,ebx
	je @start2
	mov eax,0
	jmp @end2
	@start2:
	mov eax,1
	@end2:
	cmp eax,1
	jne @false5
	mov eax,a_simpleCalculator
	mov ebx,b_simpleCalculator
	sub eax,ebx
	jmp @exit
	@false5:
	mov eax,n_simpleCalculator
	mov ebx,3
	cmp eax,ebx
	je @start3
	mov eax,0
	jmp @end3
	@start3:
	mov eax,1
	@end3:
	cmp eax,1
	jne @false6
	mov eax,a_simpleCalculator
	mov ebx, b_simpleCalculator
	mul ebx
	jmp @exit
	@false6:
	mov eax,n_simpleCalculator
	mov ebx,4
	cmp eax,ebx
	je @start4
	mov eax,0
	jmp @end4
	@start4:
	mov eax,1
	@end4:
	cmp eax,1
	jne @false7
	mov eax,a_simpleCalculator
	mov ecx,b_simpleCalculator
	cdq
	idiv ecx
	jmp @exit
	@false7:
	mov eax,n_simpleCalculator
	mov ebx,5
	cmp eax,ebx
	je @start5
	mov eax,0
	jmp @end5
	@start5:
	mov eax,1
	@end5:
	cmp eax,1
	jne @false8
	mov eax,a_simpleCalculator
	mov ebx,b_simpleCalculator
	cmp eax,ebx
	je @start6
	mov eax,0
	jmp @end6
	@start6:
	mov eax,1
	@end6:
	jmp @exit
	@false8:
	mov eax,n_simpleCalculator
	mov ebx,6
	cmp eax,ebx
	je @start7
	mov eax,0
	jmp @end7
	@start7:
	mov eax,1
	@end7:
	cmp eax,1
	jne @false9
	mov eax,a_simpleCalculator
	mov ebx, b_simpleCalculator
	cmp eax,ebx
	je @true8
	mov eax,1
	jmp @exit8
	@true8:
	mov eax,0
	@exit8:
	jmp @exit
	@false9:
	mov eax,n_simpleCalculator
	mov ebx,7
	cmp eax,ebx
	je @start9
	mov eax,0
	jmp @end9
	@start9:
	mov eax,1
	@end9:
	cmp eax,1
	jne @false10
	mov eax,a_simpleCalculator
	mov ebx,b_simpleCalculator
	or eax,ebx
	jmp @exit
	@false10:
	mov eax,n_simpleCalculator
	mov ebx,8
	cmp eax,ebx
	je @start10
	mov eax,0
	jmp @end10
	@start10:
	mov eax,1
	@end10:
	cmp eax,1
	jne @false11
	mov eax,a_simpleCalculator
	mov ebx,b_simpleCalculator
	xor eax,ebx
	jmp @exit
	@false11:
	mov eax,n_simpleCalculator
	mov ebx,9
	cmp eax,ebx
	je @start11
	mov eax,0
	jmp @end11
	@start11:
	mov eax,1
	@end11:
	cmp eax,1
	jne @false12
	mov eax,a_simpleCalculator
	mov ebx,b_simpleCalculator
	and eax,ebx
	jmp @exit
	@false12:
	mov eax,n_simpleCalculator
	mov ebx,10
	cmp eax,ebx
	je @start12
	mov eax,0
	jmp @end12
	@start12:
	mov eax,1
	@end12:
	cmp eax,1
	jne @false13
	mov eax,a_simpleCalculator
	mov ecx,b_simpleCalculator
	cdq
	idiv ecx
	jmp @exit
	@false13:
	mov eax,n_simpleCalculator
	mov ebx,11
	cmp eax,ebx
	je @start13
	mov eax,0
	jmp @end13
	@start13:
	mov eax,1
	@end13:
	cmp eax,1
	jne @false14
	mov eax,a_simpleCalculator
	mov ebx,b_simpleCalculator
	cmp eax,0
	je @start14
	jmp @end14
	@start14:
	mov eax,ebx
	@end14:
	jmp @exit
	@false14:
	mov eax,n_simpleCalculator
	mov ebx,12
	cmp eax,ebx
	je @start15
	mov eax,0
	jmp @end15
	@start15:
	mov eax,1
	@end15:
	cmp eax,1
	jne @false15
	mov eax,a_simpleCalculator
	mov ebx,b_simpleCalculator
	cmp eax,0
	je @start16
	cmp ebx,0
	je @start16_16
	mov eax,ebx
	jmp @end16
	@start16:
	@start16_16:
	mov eax,0
	@end16:
	jmp @exit
	@false15:
	mov eax,n_simpleCalculator
	mov ebx,13
	cmp eax,ebx
	je @start17
	mov eax,0
	jmp @end17
	@start17:
	mov eax,1
	@end17:
	cmp eax,1
	jne @false16
	mov eax,a_simpleCalculator
	mov ebx,b_simpleCalculator
	cmp eax,ebx
	jl @lesser18
	mov eax,0
	jmp @exit_lesser18
	@lesser18:
	mov eax,1
	@exit_lesser18:
	jmp @exit
	@false16:
	mov eax,n_simpleCalculator
	mov ebx,14
	cmp eax,ebx
	je @start19
	mov eax,0
	jmp @end19
	@start19:
	mov eax,1
	@end19:
	cmp eax,1
	jne @false17
	mov eax,a_simpleCalculator
	mov ebx,b_simpleCalculator
	cmp eax,ebx
	jle @les_eq20
	mov eax,0
	jmp @exit_les_eq20
	@les_eq20:
	mov eax,1
	@exit_les_eq20:
	jmp @exit
	@false17:
	mov eax,n_simpleCalculator
	mov ebx,15
	cmp eax,ebx
	je @start21
	mov eax,0
	jmp @end21
	@start21:
	mov eax,1
	@end21:
	cmp eax,1
	jne @false18
	mov eax,a_simpleCalculator
	mov ebx,b_simpleCalculator
	cmp eax,ebx
	jg @bigger22
	mov eax,0
	jmp @exit_bigger22
	@bigger22:
	mov eax,1
	@exit_bigger22:
	jmp @exit
	@false18:
	mov eax,n_simpleCalculator
	mov ebx,16
	cmp eax,ebx
	je @start23
	mov eax,0
	jmp @end23
	@start23:
	mov eax,1
	@end23:
	cmp eax,1
	jne @false19
	mov eax,a_simpleCalculator
	mov ebx,b_simpleCalculator
	cmp eax,ebx
	jge @big_eq24
	mov eax,0
	jmp @exit_big_eq24
	@big_eq24:
	mov eax,1
	@exit_big_eq24:
	jmp @exit
	@false19:
	mov eax,0
@exit:
	mov answer_simpleCalculator,eax
	mov eax,answer_simpleCalculator
	mov solution_simpleCalculator,eax
	ret
simpleCalculator endp


invoke main
invoke ExitProcess, 0

END start
