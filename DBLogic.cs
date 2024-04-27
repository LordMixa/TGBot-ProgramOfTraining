using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teleg_training.DBEntities;
using Teleg_training.Repository;

namespace Teleg_training
{
    internal class DBLogic
    {
        ProgramListContext context;
        IMapper mapper;
        MapperConfiguration config;
        public DBLogic(ProgramListContext programListContext)
        {
            context = programListContext;
            config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new UserMappingProfile());
            });
            mapper = config.CreateMapper();
        }
        public (List<ModelList>, List<ProductModel>) GetLists()
        {
            using (var programList = new ProgramListRepository(context))
            {
                using (var authors = new AuthorRepository(context))
                {
                    using (var product = new ProductRepository(context))
                    {
                        List<DBAuthor> dBauthors = authors.GetAll().ToList();
                        List<DBProgramList> dBProgramLists = programList.GetAll().ToList();

                        List<ModelList> listprog = mapper.Map<List<ModelList>>(dBProgramLists);
                        for (int i = 0; i < dBProgramLists.Count; i++)
                        {
                            dBProgramLists[i].Author = dBauthors.Find(x => x.AuthorId == dBProgramLists[i].AuthorId);
                        }
                        List<DBProduct> dBProducts = product.GetAll().ToList();
                        List<ProductModel> listprod = mapper.Map<List<ProductModel>>(dBProducts);
                        return (listprog,listprod);
                    }
                }
            }
        }
        public void LikeProgram(ModelList model)
        {

        }
        //public List<ProductModel> GetProductList()
        //{
        //    using (var product = new ProductRepository(context))
        //    {
        //        List<DBProduct> dBProducts = product.GetAll().ToList();
        //        List<ProductModel> list = mapper.Map<List<ProductModel>>(dBProducts);
        //        return list;
        //    }
        //}
    }
}
