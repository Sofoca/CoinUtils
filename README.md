# CoinUtils
This library will bundle lightweight (as in no non-system dependencies) utility classes for working with Bitcoin and similar Altcoins in C#/.NET Core. Adding Bitcoin-derived Altcoins is fairly straighforward and pull requests (prefereably including test cases) are very appreciated. The library might support fundamentally different cryptocurrency protocols in the future, especially when no feasible other alternative for those use cases exists. However, feasible alternatives for e.g. address validation exist at least for [Ethereum](#ethereum) and [Ripple](#ripple). 

## How to
…integrate this library into your project? Currently you have to manually add files or code to your project in order to use this library. I plan on publishing this library on NuGet sometime soon™.  

## AddressValidator
This class validates the correctness of the encoding, the checksum and some magic numbers of address. It currently supports all Base58 and Bech32 encoded addresses in Bitcoin and Litecoin. An example use case is validating addresses supplied by users for withdrawals before passing them to other software components. 

### Usage
Call `AdressValidator.IsValidAddress(address, currency)` with `address` being self-explanatory and `currency` being either `"btc"` or `"ltc"`. Call `AdressValidator.IsValidAddress(address, currency, true)` for validating Testnet addresses. 

### Alternatives
#### Ethereum
```csharp
Nethereum.Util.AddressUtil.Current.IsValidEthereumAddressHexFormat(address) 
        && (Nethereum.Util.AddressUtil.Current.IsChecksumAddress(address) // either correct checksum present
            || address == address.ToLower() // or all lowercase
            || address.Substring(2) == address.Substring(2).ToUpper()) // or all except '0x' uppercase
```
[Nethereum](https://nethereum.com/) validates Ethereum addresses.

#### Ripple
```csharp
Ripple.Address.AddressCodec.IsValidAddress(address)
```
[Ripple.NetCore](https://github.com/chriswill/ripple-netcore) validates Ripple addresses. However, it validates strictly offline, so it does not check if the [`RequireDest` flag](https://developers.ripple.com/become-an-xrp-ledger-gateway.html#requiredest) is set or if the [reserve requirements](https://developers.ripple.com/reserves.html) are met.