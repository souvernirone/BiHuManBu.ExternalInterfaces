//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: System.Data.Mapping.EntityViewGenerationAttribute(typeof(Edm_EntityMappingGeneratedViews.ViewsForBaseEntitySets9DD1011C2A2CBDD12BF8CFE99C69FFF200759CFDEEC14430E0D0398802B24646))]

namespace Edm_EntityMappingGeneratedViews
{
    
    
    /// <Summary>
    /// 此类型包含在设计时生成的 EntitySets 和 AssociationSets 的视图。
    /// </Summary>
    public sealed class ViewsForBaseEntitySets9DD1011C2A2CBDD12BF8CFE99C69FFF200759CFDEEC14430E0D0398802B24646 : System.Data.Mapping.EntityViewContainer
    {
        
        /// <Summary>
        /// 构造函数存储各区的视图，以及根据元数据和映射结束和视图生成的哈希值。
        /// </Summary>
        public ViewsForBaseEntitySets9DD1011C2A2CBDD12BF8CFE99C69FFF200759CFDEEC14430E0D0398802B24646()
        {
            this.EdmEntityContainerName = "eftestEntities";
            this.StoreEntityContainerName = "eftestModelStoreContainer";
            this.HashOverMappingClosure = "b697cde8bfd60f5537c0d34c9f4cc3d4890fe16b234bf09195bcb89c3decf324";
            this.HashOverAllExtentViews = "832b56c549572e9b10f44c063e5d763d569ea2bbf53f8492c948fec0af7971c1";
            this.ViewCount = 2;
        }
        
        /// <Summary>
        /// 此方法返回给定索引的视图。
        /// </Summary>
        protected override System.Collections.Generic.KeyValuePair<string, string> GetViewAt(int index)
        {
            if ((index == 0))
            {
                return GetView0();
            }
            if ((index == 1))
            {
                return GetView1();
            }
            throw new System.IndexOutOfRangeException();
        }
        
        /// <Summary>
        /// 返回 eftestModelStoreContainer.student 的视图
        /// </Summary>
        private System.Collections.Generic.KeyValuePair<string, string> GetView0()
        {
            return new System.Collections.Generic.KeyValuePair<string, string>("eftestModelStoreContainer.student", @"
    SELECT VALUE -- Constructing student
        [eftestModel.Store.student](T1.student_id, T1.student_Name, T1.student_Age)
    FROM (
        SELECT 
            T.id AS student_id, 
            T.Name AS student_Name, 
            T.Age AS student_Age, 
            True AS _from0
        FROM eftestEntities.student AS T
    ) AS T1");
        }
        
        /// <Summary>
        /// 返回 eftestEntities.student 的视图
        /// </Summary>
        private System.Collections.Generic.KeyValuePair<string, string> GetView1()
        {
            return new System.Collections.Generic.KeyValuePair<string, string>("eftestEntities.student", @"
    SELECT VALUE -- Constructing student
        [eftestModel.student](T1.student_id, T1.student_Name, T1.student_Age)
    FROM (
        SELECT 
            T.id AS student_id, 
            T.Name AS student_Name, 
            T.Age AS student_Age, 
            True AS _from0
        FROM eftestModelStoreContainer.student AS T
    ) AS T1");
        }
    }
}
