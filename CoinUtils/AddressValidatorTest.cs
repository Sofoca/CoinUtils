using System;
using Xunit;
using Xunit.Abstractions;

namespace CoinUtils
{
    public class AddressValidatorTest
    {
        private readonly ITestOutputHelper output;

        public AddressValidatorTest(ITestOutputHelper output)
        {
            this.output = output;
        }
        
        private static string[] _invalidBech32Addresses = {
            "tc1qw508d6qejxtdg4y5r3zarvary0c5xw7kg3g4ty",
            "bc1qw508d6qejxtdg4y5r3zarvary0c5xw7kv8f3t5",
            "BC13W508D6QEJXTDG4Y5R3ZARVARY0C5XW7KN40WF2",
            "bc1rw5uspcuh",
            "bc10w508d6qejxtdg4y5r3zarvary0c5xw7kw508d6qejxtdg4y5r3zarvary0c5xw7kw5rljs90",
            "BC1QR508D6QEJXTDG4Y5R3ZARVARYV98GJ9P",
            "tb1qrp33g0q5c5txsp9arysrx4k6zdkfs4nce4xj0gdcccefvpysxf3q0sL5k7",
            "tb1pw508d6qejxtdg4y5r3zarqfsj6c3",
            "tb1qrp33g0q5c5txsp9arysrx4k6zdkfs4nce4xj0gdcccefvpysxf3pjxtptv",
            "bc1pw508d6qeJxtdg4y5r3zarvary0c5xw7kw508d6qejxtdg4y5r3zarvary0c5xw7k7grplx"
        };
        
        private static string[][] _validBech32AddressWithScriptPubKey = {
            new [] { "BC1QW508D6QEJXTDG4Y5R3ZARVARY0C5XW7KV8F3T4", "0014751e76e8199196d454941c45d1b3a323f1433bd6"},
            new [] { "bc1pw508d6qejxtdg4y5r3zarvary0c5xw7kw508d6qejxtdg4y5r3zarvary0c5xw7k7grplx", "5128751e76e8199196d454941c45d1b3a323f1433bd6751e76e8199196d454941c45d1b3a323f1433bd6"},
            new [] { "BC1SW50QA3JX3S", "6002751e"},
            new [] { "bc1zw508d6qejxtdg4y5r3zarvaryvg6kdaj", "5210751e76e8199196d454941c45d1b3a323"}
        };

        private static string[][] _validBech32TestnetAddressWithScriptPubKey =
        {
            new[]
            {
                "tb1qqqqqp399et2xygdj5xreqhjjvcmzhxw4aywxecjdzew6hylgvsesrxh6hy",
                "0020000000c4a5cad46221b2a187905e5266362b99d5e91c6ce24d165dab93e86433"
            },
            new[]
            {
                "tb1qrp33g0q5c5txsp9arysrx4k6zdkfs4nce4xj0gdcccefvpysxf3q0sl5k7",
                "00201863143c14c5166804bd19203356da136c985678cd4d27a1b8c6329604903262"
            }
        };

        [Fact]
        public void EmptyBtcAddress()
        {
            Assert.ThrowsAny<ArgumentException>(() => AddressValidator.IsValidAddress("", "btc"));
        }
        [Fact]
        public void nullBtcAddress()
        {
            Assert.ThrowsAny<ArgumentException>(() => AddressValidator.IsValidAddress(null, "btc"));
        }

        [Fact]
        public void GoodBtcAddress()
        {
            Assert.True(AddressValidator.IsValidAddress("1comboyNsev2ubWRbPZpxxNhghLfonzuN", "btc"));
        }

        [Fact]
        public void GoodBtcAddressesBech32()
        {
            foreach (var address in _validBech32AddressWithScriptPubKey)
                Assert.True(AddressValidator.IsValidAddress(address[0], "btc"));
        }
        
        [Fact]
        public void GoodBtcAddressesBech32Testnet()
        {
            foreach (var address in _validBech32TestnetAddressWithScriptPubKey)
            {
                output.WriteLine(address[0]);
                Assert.True(AddressValidator.IsValidAddress(address[0], "btc", true));
            }
        }

        [Fact]
        public void BadBtcAddressesBech32()
        {
            foreach (var address in _invalidBech32Addresses)
            {
                output.WriteLine(address);
                Assert.ThrowsAny<FormatException>(() => AddressValidator.IsValidAddress(address, "btc"));
            }

        }
        
        [Fact]
        public void GoodLtcAddressBech32()
        {
            Assert.True(AddressValidator.IsValidAddress("ltc1qz4ptnv9cu95zm0z97vsnavmyu8t6pk383k87dx", "ltc"));
        }

        [Fact]
        public void GoodBtcAddressP2SH()
        {
            Assert.True(AddressValidator.IsValidAddress("3J98t1WpEZ73CNmQviecrnyiWrnqRhWNLy", "btc"));
        }

        [Fact]
        public void GoodBtcAddressP2PKH()
        {
            Assert.True(AddressValidator.IsValidAddress("1BvBMSEYstWetqTFn5Au4m4GFg7xJaNVN2", "btc"));
        }

        [Fact]
        public void BadBtcAddressActuallyLitecoinLegacy()
        {
            Assert.ThrowsAny<FormatException>(() => AddressValidator.IsValidAddress("LgADTx6JrydCVdrrhJ8wkFkXdx3UszKsFx", "btc"));
        }

        [Fact]
        public void BadBtcAddressActuallyLitecoinSegwitLegacy()
        {
            // cannot be distinguished due to protocol-level constraints
            Assert.True(AddressValidator.IsValidAddress("3JEE5m1NaUCKCTXwyPkRhwiKzJUaKJDsJi", "btc"));
        }

        [Fact]
        public void BadBtcAddressActuallyLitecoinSegwitNew()
        {
            Assert.True(AddressValidator.IsValidAddress("MAP2uc4aFVwJwoJp3p8yFMs7zy6Pa5e9Zv", "ltc"));
        }

        [Fact]
        public void GoodLtcLAddress()
        {
            Assert.True(AddressValidator.IsValidAddress("MAP2uc4aFVwJwoJp3p8yFMs7zy6Pa5e9Zv", "ltc"));
        }

        [Fact]
        public void GoodLtc3SegwitLegacyAddress()
        {
            Assert.True(AddressValidator.IsValidAddress("MAP2uc4aFVwJwoJp3p8yFMs7zy6Pa5e9Zv", "ltc"));
        }

        [Fact]
        public void GoodLtcSegwitNewAddress()
        {
            Assert.True(AddressValidator.IsValidAddress("MAP2uc4aFVwJwoJp3p8yFMs7zy6Pa5e9Zv", "ltc"));
        }

        [Fact]
        public void GoodBtcTestnetAddress()
        {
            Assert.True(AddressValidator.IsValidAddress("mxXBpKSowZZeA6eCxpHYygVBnJpskNWwMN", "btc", true));
        }

        [Fact]
        public void BadBtcAddressTestnet()
        {
            Assert.ThrowsAny<FormatException>(() => AddressValidator.IsValidAddress("mxXBpKSowZZeA6eCxpHYygVBnJpskNWwMN", "ltc"));
        }

        [Fact]
        public void GoodLtcTestnetAddress()
        {
            Assert.True(AddressValidator.IsValidAddress("mjFBdzsYNBCeabLNwyYYCt8epG7GhzYeTw", "ltc", true));
        }

        [Fact]
        public void BadLtcAddressTestnet()
        {
            Assert.ThrowsAny<FormatException>(() => AddressValidator.IsValidAddress("mjFBdzsYNBCeabLNwyYYCt8epG7GhzYeTw", "ltc"));
        }
    }
}