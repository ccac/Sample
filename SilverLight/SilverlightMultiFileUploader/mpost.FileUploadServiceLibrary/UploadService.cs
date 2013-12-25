using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.IO;
using System.Web.Hosting;
using System.ServiceModel.Activation;

/*
 * Copyright Michiel Post
 * http://www.michielpost.nl
 * contact@michielpost.nl
 * */

namespace mpost.FileUploadServiceLibrary
{
    [AspNetCompatibilityRequirements  (RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class UploadService : IUploadService
    {
        private string _tempExtension = "_temp";

        #region IUploadService Members

        
        /// <summary>
        /// 取消上传
        /// </summary>
        /// <param name="fileName"></param>
        public void CancelUpload(string fileName)
        {
            string uploadFolder = GetUploadFolder();
            string tempFileName = fileName + _tempExtension;

            if (File.Exists(@HostingEnvironment.ApplicationPhysicalPath + "/" + uploadFolder + "/" + tempFileName))
                File.Delete(@HostingEnvironment.ApplicationPhysicalPath + "/" + uploadFolder + "/" + tempFileName);
        }

        public void StoreFileAdvanced(string fileName, byte[] data, int dataLength, string parameters, bool firstChunk, bool lastChunk)
        {
            string uploadFolder = GetUploadFolder();
            string tempFileName = fileName + _tempExtension;

            //当上传文件的第一批数据时，先清空以往的相同文件名的文件（同名文件可能为上传失败造成）
            if (firstChunk)
            {
                //删除临时文件
                if (File.Exists(@HostingEnvironment.ApplicationPhysicalPath + "/" + uploadFolder + "/" + tempFileName))
                    File.Delete(@HostingEnvironment.ApplicationPhysicalPath + "/" + uploadFolder + "/" + tempFileName);

                //删除目标文件
                if (File.Exists(@HostingEnvironment.ApplicationPhysicalPath + "/" + uploadFolder + "/" + fileName))
                    File.Delete(@HostingEnvironment.ApplicationPhysicalPath + "/" + uploadFolder + "/" + fileName);

            }


            FileStream fs = File.Open(@HostingEnvironment.ApplicationPhysicalPath + "/" + uploadFolder + "/" + tempFileName, FileMode.Append);
            fs.Write(data, 0, dataLength);
            fs.Close();

            if (lastChunk)
            {
                //将临时文件重命名为原来的文件名称
                File.Move(HostingEnvironment.ApplicationPhysicalPath + "/" + uploadFolder + "/" + tempFileName, HostingEnvironment.ApplicationPhysicalPath + "/" + uploadFolder + "/" + fileName);

                //Finish stuff....
                FinishedFileUpload(fileName, parameters);
            }

        }

        /// <summary>
        /// 删除上传文件
        /// </summary>
        /// <param name="fileName"></param>
        protected void DeleteUploadedFile(string fileName)
        {
            string uploadFolder = GetUploadFolder();

            if (File.Exists(@HostingEnvironment.ApplicationPhysicalPath + "/" + uploadFolder + "/" + fileName))
                File.Delete(@HostingEnvironment.ApplicationPhysicalPath + "/" + uploadFolder + "/" + fileName);
        }

        protected virtual void FinishedFileUpload(string fileName, string parameters)
        {
        }

        /// <summary>
        /// 获取上传路径
        /// </summary>
        /// <returns></returns>
        protected virtual string GetUploadFolder()
        {
            return "Upload";
        }     


        #endregion
    }
}
