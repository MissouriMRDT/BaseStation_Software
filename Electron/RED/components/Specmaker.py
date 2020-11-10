# program to test the functionally of the Sepctrometer.tsx component
import sys

def line(x):
	return x

def Expo(x):
	return x**2

def Cubic(x):
	return x**3

if sys.argv[1] == '-l':
	ops = line

if sys.argv[1] == '-e':
	ops = Expo
if sys.argv[1] == '-c':
	ops = Cubic

spec = open("spec2.tsx","w")
spec.write("export const specData: { x: number; y: number }[] = [")
for x in range(300):
	y = ops(x)
	spec.write("\n  { x: " + str(x) + ", y: " + str(y) +" },")
spec.write("]")
spec.close()
