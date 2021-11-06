// SPDX-License-Identifier: MIT
// OpenZeppelin Contracts v4.3.2 (token/ERC1155/presets/ERC1155PresetMinterPauser.sol)

pragma solidity ^0.8.0;

import "https://github.com/OpenZeppelin/openzeppelin-contracts/blob/master/contracts/token/ERC1155/extensions/ERC1155Supply.sol";
import "https://github.com/OpenZeppelin/openzeppelin-contracts/blob/master/contracts/access/Ownable.sol";

contract EWO_Nfts is ERC1155Supply, Ownable {
    // Obstacle ids
    uint256 public constant GREY_DUSTBIN = 0;
    uint256 public constant RED_BAR = 1;
    uint256 public constant CONE = 2;
    
    // Mapping from obstacle id to token ID to owner address
    mapping(uint256 => mapping (uint256 => address)) private _owners;
    
    constructor() ERC1155("https://token-cdn-domain/") {
        _mint(msg.sender, GREY_DUSTBIN, 10, "");
        _mint(msg.sender, RED_BAR, 10, "");
        _mint(msg.sender, CONE, 10, "");
    }
    
    function buyObstacle(uint256 obstacle_id) public view returns(uint256) {
        require(balanceOf(msg.sender, obstacle_id) < 2);
        return balanceOf(address(this),0);
    }
    
    function withdrawObstacles(uint256 obstacle_id) public onlyOwner() {
        uint256 obstacles = balanceOf(address(this), obstacle_id);
        address owner = owner();
        this.safeTransferFrom(address(this), owner, obstacle_id, obstacles, "");
    }
    
    // function ownerOf(uint256 tokenId) public view virtual override returns (address) {
    //     address owner = _owners[tokenId];
    //     require(owner != address(0), "ERC721: owner query for nonexistent token");
    //     return owner;
    // }
    
    
}