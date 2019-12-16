using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;
using System.IO;

namespace matchImage
{
    class Program
    {
        static void Main(string[] args)
        {
            Program p = new Program();

            string sourceFolderPath = @"";
            string matchFolderPath = @"";

            foreach (string result in p.MatchFile(sourceFolderPath, matchFolderPath))
            {
                Console.WriteLine(result);
            }
        }

        private Double getMax(List<double> list)
        {
            double max = 0;
            foreach (double x in list)
            {
                if (x > max)
                {
                    max = x;
                }
            }
            return max;
        }

        /// <summary>
        /// 이미지 매칭
        /// </summary>
        /// <param name="sourceFolderPath">소스 이미지들이 들어있는 디렉토리 경로</param>
        /// <param name="matchFolderPath">매칭 이미지들이 들어있는 디렉토리 경로</param>
        /// <returns></returns>
        private List<string> MatchFile(string sourceFolderPath, string matchFolderPath)
        {
            List<string> saveMatchInfos = new List<string>();

            System.IO.DirectoryInfo sourceDir = new System.IO.DirectoryInfo(sourceFolderPath);
            System.IO.DirectoryInfo matchDir = new System.IO.DirectoryInfo(matchFolderPath);

            foreach(System.IO.FileInfo sourcefile in sourceDir.GetFiles())
            {
                List<string> matchFiles = new List<string>();
                List<Double> resultList = new List<double>();

                foreach (System.IO.FileInfo matchfile in matchDir.GetFiles())
                {
                    if (!matchfile.FullName.Contains("Thumbs.db"))
                    {
                        double result = MatchImage(sourcefile.FullName, matchfile.FullName);
                        matchFiles.Add(matchfile.Name);
                        resultList.Add(result);
                    }
                    else
                    {
                        continue;
                    }
                }
                double maxResult = getMax(resultList);
                int num = resultList.IndexOf(maxResult);
                string maxMatchFile = matchFiles[num];

                string saveMatchInfo = sourcefile.Name + "과 유사도가 가장 높은 파일은" + maxMatchFile + "입니다. [이미지 유사도 : " + maxResult + "]";

                Console.WriteLine(saveMatchInfo);
                saveMatchInfos.Add(saveMatchInfo);
            }

            return saveMatchInfos;
        }

        /// <summary>
        /// 이미지 매칭 함수
        /// </summary>
        /// <param name="source">소스 이미지 경로</param>
        /// <param name="match">매칭할 이미지 경로</param>
        /// <returns></returns>
        private double MatchImage(string source, string match)
        {
            // 원본 이미지
            Mat src = Cv2.ImRead(source);
            // 찾을 이미지
            Mat findImage = Cv2.ImRead(match);

            Mat re2 = src.MatchTemplate(findImage, TemplateMatchModes.CCoeffNormed);

            double minval, maxval = 0;

            OpenCvSharp.Point minloc, maxloc;

            Cv2.MinMaxLoc(re2, out minval, out maxval, out minloc, out maxloc);

            return maxval;
        }
    }
}
