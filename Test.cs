namespace BallExtractor
{
static class Program
{
static void Main(string[] args)        
{            
int counter = 0;                   
Console.Write("Введите размер массива: ");           
int n = int.Parse(Console.ReadLine());                     
int[] arr = new int[n];            
Random rnd = new Random();        
for (int i = 0; i < arr.Length; i++)            
{                
arr[i] = rnd.Next(101) - 50;            
}                              
for (int i = 0; i < arr.Length; i++)            
{                
Console.Write("{0} ", arr[i]);            
}            
Console.WriteLine();                   
Console.WriteLine("Первые 5 минимальных элементов массива:");            
int Min = FirstMinElement(arr);                       
for (int i = 0; i < arr.Length; i++)                
{                    
if (arr[i] <= Min && arr.Length >= 5 && counter < 5)                    
{                        
counter += 1;                        
Console.Write(arr[i] + " ");                     
}                
}            
Console.ReadKey();        
}        
static int FirstMinElement(int[] arr)        
{            
int[] sortArr = (int[])arr.Clone();            
Array.Sort(sortArr);            
if (sortArr.Length < 5)            
{                
Console.WriteLine("Не достаточно элементов");                
return 0;            
}            
else return sortArr[4];        
}
}
}