using System.Collections.Generic;
using BiHuManBu.ExternalInterfaces.Models;
using BiHuManBu.ExternalInterfaces.Services.Interfaces;
using BiHuManBu.ExternalInterfaces.Services.Messages.Request;

namespace BiHuManBu.ExternalInterfaces.Services
{
    public class ImagesService:IImagesService
    {
        public IImagesRepository _ImagesRepository;

        public ImagesService(IImagesRepository imagesRepository)
        {
            _ImagesRepository = imagesRepository;
        }
        public int Add(GetImageRequest request)
        {
            int response = 0;
            List<bx_images> bxImageses=new List<bx_images>();
            if (request != null)
            {
                if (request.yancheimgs != null && request.yancheimgs.Count > 0)
                {
                    foreach (var item in request.yancheimgs)
                    {
                        bx_images bxImages = new bx_images();
                        bxImages.buid = request.buid;
                        bxImages.image = item;
                        bxImages.type = 1;
                        bxImageses.Add(bxImages);
                    }
                }
                if (!string.IsNullOrEmpty(request.zjimg))
                {
                    bx_images bxImages = new bx_images();
                    bxImages.buid = request.buid;
                    bxImages.image = request.zjimg;
                    bxImages.type = 2;
                    bxImageses.Add(bxImages);
                }
                if (bxImageses.Count > 0)
                {
                    response = _ImagesRepository.Add(bxImageses);
                }
            }
            
            return response;
        }

        public List<bx_images> GetImages(long buid)
        {
            return _ImagesRepository.FindByBuid(buid);
        }
    }
}
