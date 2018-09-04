using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace IVWIN
{
    class FileSort
    {
        public const int SORT_BY_NAME = 0;
        public const int SORT_BY_NAME_DESC = 1;
        public const int SORT_BY_DATE = 2;
        public const int SORT_BY_DATE_DESC = 3;
        public const int SORT_BY_SIZE = 4;
        public const int SORT_BY_SIZE_DESC = 5;
        public const int SORT_DEAULT = SORT_BY_NAME;

        static public void Sort(ref List<FileSystemInfo> vs, int sortOption) {
            switch (sortOption)
            {
                case FileSort.SORT_BY_NAME:
                    SortByName(ref vs);
                    break;
                case FileSort.SORT_BY_NAME_DESC:
                    SortByNameDesc(ref vs);
                    break;
                case FileSort.SORT_BY_DATE:
                    SortByDate(ref vs);
                    break;
                case FileSort.SORT_BY_DATE_DESC:
                    SortByDateDesc(ref vs);
                    break;
                case FileSort.SORT_BY_SIZE:
                    SortBySize(ref vs);
                    break;
                case FileSort.SORT_BY_SIZE_DESC:
                    SortBySizeDesc(ref vs);
                    break;

            }
        }

        static public void SortByName (ref List<FileSystemInfo> vs){
            vs.Sort(delegate (FileSystemInfo x, FileSystemInfo y)
                {
                    return x.Name.CompareTo(y.Name);
                });
       }

        static public void SortByNameDesc (ref List<FileSystemInfo> vs)
        {
            vs.Sort(delegate (FileSystemInfo x, FileSystemInfo y)
                {
                    return y.Name.CompareTo(x.Name);
                });
       }


       static public void SortByDate(ref List<FileSystemInfo> vs)
       {
            vs.Sort(delegate (FileSystemInfo x, FileSystemInfo y)
            {
                return x.CreationTime.CompareTo(y.CreationTime);
            });
        }

        static public void SortByDateDesc(ref List<FileSystemInfo> vs)
        {
            vs.Sort(delegate (FileSystemInfo x, FileSystemInfo y)
            {
                return y.CreationTime.CompareTo(x.CreationTime);
            });

        }

        static public void SortBySize(ref List<FileSystemInfo> vs)
        {
            vs.Sort(delegate (FileSystemInfo x, FileSystemInfo y)
            {
                if ((x.Attributes & FileAttributes.Directory)== FileAttributes.Directory ) 
                {
                        if ((y.Attributes & FileAttributes.Directory) == FileAttributes.Directory)
                        {
                            return x.Name.CompareTo(y.Name); ;
                        }
                        return -1;
                }

                if ((y.Attributes & FileAttributes.Directory) == FileAttributes.Directory ) {
                   return 1;
                }

                FileInfo xx = new FileInfo(x.FullName);
                FileInfo yy = new FileInfo(y.FullName);
                return xx.Length.CompareTo(yy.Length);
            });
        }

        static public void SortBySizeDesc(ref List<FileSystemInfo> vs)
        {
            vs.Sort(delegate (FileSystemInfo y, FileSystemInfo x)
            {
                if ((x.Attributes & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    if ((y.Attributes & FileAttributes.Directory) == FileAttributes.Directory)
                    {
                        return x.Name.CompareTo(y.Name); ;
                    }
                    return -1;
                }

                if ((y.Attributes & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    return 1;
                }

                FileInfo xx = new FileInfo(x.FullName);
                FileInfo yy = new FileInfo(y.FullName);
                return xx.Length.CompareTo(yy.Length);
            });
        }

    }
}
