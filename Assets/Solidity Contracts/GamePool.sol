// SPDX-License-Identifier: MIT
// OpenZeppelin Contracts v4.3.2 (token/ERC1155/presets/ERC1155PresetMinterPauser.sol)

pragma solidity ^0.8.0;

// import "https://github.com/OpenZeppelin/openzeppelin-contracts/blob/master/contracts/finance/PaymentSplitter.sol";
import "https://github.com/OpenZeppelin/openzeppelin-contracts/blob/master/contracts/token/ERC1155/utils/ERC1155Holder.sol";
import "https://github.com/OpenZeppelin/openzeppelin-contracts/blob/master/contracts/access/Ownable.sol";
import "./EWO_Nfts.sol";
import "https://github.com/OpenZeppelin/openzeppelin-contracts/blob/master/contracts/token/ERC1155/IERC1155.sol";
import "https://github.com/OpenZeppelin/openzeppelin-contracts/blob/master/contracts/token/ERC1155/ERC1155.sol";
import "https://github.com/OpenZeppelin/openzeppelin-contracts/blob/master/contracts/token/ERC1155/utils/ERC1155Receiver.sol";

contract GamePool is ERC1155Receiver, Ownable{
    IERC1155 nftcontract;
    
    // Mapping from obstacle_id to Owner address to token_id 
    mapping(uint256 => mapping (address => uint256)) public _tokenid;
    
    // Mapping from obstacle_id to obstacles sold 
    mapping (uint256 => uint256) public _sold;
    
    // Mapping from obstacle id to token ID to owner address
    mapping(uint256 => mapping (uint256 => address)) public _owners;
    
    constructor(address tokenAddress){
         nftcontract = IERC1155(tokenAddress);
    }
    
    function onERC1155Received(
        address,
        address,
        uint256,
        uint256,
        bytes memory
    ) public virtual override returns (bytes4) {
        return this.onERC1155Received.selector;
    }

    function onERC1155BatchReceived(
        address,
        address,
        uint256[] memory,
        uint256[] memory,
        bytes memory
    ) public virtual override returns (bytes4) {
        return this.onERC1155BatchReceived.selector;
    }
    
    function withdrawObstacles(uint256 obstacle_id) public onlyOwner{
        uint256 obstacles = (nftcontract).balanceOf(address(this), obstacle_id);
        address owner = owner();
        (nftcontract).safeTransferFrom(address(this), owner, obstacle_id, obstacles, "");
    }
    
    function buyObstacle(uint256 obstacle_id) public {
        require((nftcontract).balanceOf(msg.sender, obstacle_id) < 2);
        uint256 tokenid = _sold[obstacle_id];
        _tokenid[obstacle_id][msg.sender] = tokenid; // token id
        _owners[obstacle_id][tokenid] = msg.sender;  // to keep track of owners of each tokenid
        _sold[obstacle_id] += 1;
        (nftcontract).safeTransferFrom(address(this), msg.sender, obstacle_id, 1, "");
    }
}