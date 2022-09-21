using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateCSV
{
    public class DRYDataReader
    {
        private List<DRYData> dryData = new List<DRYData>();

        public DRYDataReader(string filePathOfFolder, string targetFilePath)
        {
            string[] files = getAllFilesInFolder(filePathOfFolder);

            foreach (var file in files)
            {
                dryData.Add(new DRYData(file));
            }
            int dataOffset = getMaxFileLength() + 2;

            generateNewCSVFile(dataOffset, targetFilePath);
        }



        private string[] getAllFilesInFolder(string folderPath)
        {
            string[] files = Directory.GetFiles(folderPath, "*.csv");
            foreach (var file in files)
            {
                //Debug.WriteLine(file);
            }
            return files;
        }

        private int getMaxFileLength()
        {
            int max = 0;
            foreach(DRYData data in dryData)
            {
                if(data.GetFileLength() > max)
                {
                    max = data.GetFileLength();
                }
            }
            return max;
        }

        private void generateNewCSVFile(int offset, string targetFilePath)
        {
            string delimiter = ",";
            StringBuilder sb = new StringBuilder();

            string[][] firstLine = new string[][]{
                    new string[]{"Offset", offset.ToString(), "Start", (dryData.Count+ 2).ToString()},
                };

            int l = firstLine.GetLength(0);

            for (int index = 0; index < l; index++)
                sb.AppendLine(string.Join(delimiter, firstLine[index]));


            //Writes every identifier and the corresponding index to the file

            for (int i = 0; i < dryData.Count; i++)
            {
                string[][] output = new string[][]{
                    new string[]{dryData[i].Identifier,i.ToString(), "", ""},
                };

                int length = output.GetLength(0);
                for (int index = 0; index < length; index++)
                    sb.AppendLine(string.Join(delimiter, output[index]));
               


            }

            //Appends the content of every dry data csv file

            for(int i = 0; i < dryData.Count; i++)
            {
                string[][] identifier = new string[][]{
                        new string[]{dryData[i].Identifier,"", "", ""},
                        new string[]{"{DB (F)", "Hours ON","Hours OFF","MCWB (F)"},
                };

                int length = identifier.GetLength(0);
                for (int index = 0; index < length; index++)
                    sb.AppendLine(string.Join(delimiter, identifier[index]));

                for (int j = 0; j < dryData[i].DBF.Count; j++)
                {

                    string dbf = dryData[i].DBF[j].ToString();
                    string HoursOn = dryData[i].HoursOn[j].ToString();
                    string HoursOff = dryData[i].HourseOff[j].ToString();
                    string MCWBF = "N/A";
                    if (dryData[i].MCWBF[j] != null)
                    {
                        MCWBF = dryData[i].MCWBF[j].ToString();
                    }

                    string[][] line = new string[][]{
                        new string[]{ dbf, HoursOn, HoursOff, MCWBF},
                };


                    int lineLength = line.GetLength(0);
                    for (int index = 0; index < lineLength; index++)
                        sb.AppendLine(string.Join(delimiter, line[index]));
                }
                if(dryData[i].DBF.Count < offset)
                {
                    int numberOfMissingLines = offset - dryData[i].DBF.Count;
                    for (int j = 0; j < numberOfMissingLines; j++)
                    {


                        string[][] line = new string[][]{
                        new string[]{ "", "", "", ""},
                };


                        int lineLength = line.GetLength(0);
                        for (int index = 0; index < lineLength; index++)
                            sb.AppendLine(string.Join(delimiter, line[index]));
                    }
                }



            }
           
            File.WriteAllText(targetFilePath + "/outputFile.csv", sb.ToString());


        }


        public class DRYData
        {
            public string Identifier { get; set; }

            public List<float> DBF = new List<float>();

            public List<float> HoursOn = new List<float>();

            public List<float> HourseOff = new List<float>();

            public List<float?> MCWBF = new List<float?>();

            private int fileLength { get; set; }

            public DRYData(string filePath)
            {
                readFileContent(filePath);
            }


            private void readFileContent(string filePath)
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    string currentLine;
                    currentLine = sr.ReadLine();
                    string[] split = currentLine.Split(",");
                    this.Identifier = split[0].Split(" ")[1];
                    this.fileLength = 1;
                    currentLine = sr.ReadLine();
                    this.fileLength++;

                    // currentLine will be null when the StreamReader reaches the end of file
                    while ((currentLine = sr.ReadLine()) != null)
                    {
                        //Debug.WriteLine(currentLine);
                        string[] values = currentLine.Split(",");
                        this.DBF.Add(float.Parse(values[0], CultureInfo.InvariantCulture.NumberFormat));
                        this.HoursOn.Add(float.Parse(values[1], CultureInfo.InvariantCulture.NumberFormat));
                        this.HourseOff.Add(float.Parse(values[2], CultureInfo.InvariantCulture.NumberFormat));
                        if(values[3] == "N/A")
                        {
                            this.MCWBF.Add(null);
                        } else
                        {
                            this.MCWBF.Add(float.Parse(values[3], CultureInfo.InvariantCulture.NumberFormat));
                        }
                        
                        this.fileLength++;
                    }
                }
            }

            public int GetFileLength()
            {
                return fileLength;
            }

        }


    }


}
