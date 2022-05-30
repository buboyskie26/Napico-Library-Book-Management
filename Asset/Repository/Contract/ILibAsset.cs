using Asset.Data.Model;
using Asset.ViewModel.AssetVM;
using Asset.ViewModel.DropDownVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asset.Repository.Contract
{
    public interface ILibAsset 
    {
        Task Add(LibraryAsset libraryAsset);
        Task AddAsset(CreateIndex asset);
        Task UpdateAsset(CreateIndex asset);
        IEnumerable<LibraryAsset> GetAll();

        Task<DropDown> ListDropdown();

        LibraryAsset GetById(int id);
        IEnumerable<LibraryAsset> SingleLibrary(string search);

        string GetType(int id);
    }
}
