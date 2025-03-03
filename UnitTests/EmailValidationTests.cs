//
// EmailValidationTests.cs
//
// Author: Jeffrey Stedfast <jestedfa@microsoft.com>
//
// Copyright (c) 2013-2025 Jeffrey Stedfast
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using NUnit.Framework;

using EmailValidation;

namespace UnitTests
{
	[TestFixture]
	public class EmailValidationTests
	{
		static readonly string[] ValidAddresses = {
			"\"Abc\\@def\"@example.com",
			"\"Fred Bloggs\"@example.com",
			"\"Joe\\\\Blow\"@example.com",
			"\"Abc@def\"@example.com",
			"customer/department=shipping@example.com",
			"$A12345@example.com",
			"!def!xyz%abc@example.com",
			"_somename@example.com",
			"valid@[1.1.1.1]",
			"valid.ipv4.addr@[123.1.72.10]",
			"valid.ipv4.addr@[255.255.255.255]",
			"valid.ipv6.addr@[IPv6:::]",
			"valid.ipv6.addr@[IPv6:0::1]",
			"valid.ipv6.addr@[IPv6:::12.34.56.78]",
			"valid.ipv6.addr@[IPv6:::3333:4444:5555:6666:7777:8888]",
			"valid.ipv6.addr@[IPv6:2607:f0d0:1002:51::4]",
			"valid.ipv6.addr@[IPv6:fe80::230:48ff:fe33:bc33]",
			"valid.ipv6.addr@[IPv6:fe80:0000:0000:0000:0202:b3ff:fe1e:8329]",
			"valid.ipv6v4.addr@[IPv6:::12.34.56.78]",
			"valid.ipv6v4.addr@[IPv6:aaaa:aaaa:aaaa:aaaa:aaaa:aaaa:127.0.0.1]",
			new string ('a', 64) + "@example.com", // max local-part length (64 characters)
			"valid@" + new string ('a', 63) + ".com", // max subdomain length (64 characters)
			"valid@" + new string ('a', 60) + "." + new string ('b', 60) + "." + new string ('c', 60) + "." + new string ('d', 61) + ".com", // max length (254 characters)
			new string ('a', 64) + "@" + new string ('a', 45) + "." + new string ('b', 46) + "." + new string ('c', 45) + "." + new string ('d', 46) + ".com", // max local-part length (64 characters)

			// examples from wikipedia
			"niceandsimple@example.com",
			"very.common@example.com",
			"a.little.lengthy.but.fine@dept.example.com",
			"disposable.style.email.with+symbol@example.com",
			"user@[IPv6:2001:db8:1ff::a0b:dbd0]",
			"\"much.more unusual\"@example.com",
			"\"very.unusual.@.unusual.com\"@example.com",
			"\"very.(),:;<>[]\\\".VERY.\\\"very@\\\\ \\\"very\\\".unusual\"@strange.example.com",
			"postbox@com",
			"admin@mailserver1",
			"!#$%&'*+-/=?^_`{}|~@example.org",
			"\"()<>[]:,;@\\\\\\\"!#$%&'*+-/=?^_`{}| ~.a\"@example.org",
			"\" \"@example.org",

			// examples from https://github.com/Sembiance/email-validator
			"\"\\e\\s\\c\\a\\p\\e\\d\"@sld.com",
			"\"back\\slash\"@sld.com",
			"\"escaped\\\"quote\"@sld.com",
			"\"quoted\"@sld.com",
			"\"quoted-at-sign@sld.org\"@sld.com",
			"&'*+-./=?^_{}~@other-valid-characters-in-local.net",
			"01234567890@numbers-in-local.net",
			"a@single-character-in-local.org",
			"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ@letters-in-local.org",
			"backticksarelegit@test.com",
			"bracketed-IP-instead-of-domain@[127.0.0.1]",
			"country-code-tld@sld.rw",
			"country-code-tld@sld.uk",
			"letters-in-sld@123.com",
			"local@dash-in-sld.com",
			"local@sld.newTLD",
			"local@sub.domains.com",
			"mixed-1234-in-{+^}-local@sld.net",
			"one-character-third-level@a.example.com",
			"one-letter-sld@x.org",
			"punycode-numbers-in-tld@sld.xn--3e0b707e",
			"single-character-in-sld@x.org",
			"the-character-limit@for-each-part.of-the-domain.is-sixty-three-characters.this-is-exactly-sixty-three-characters-so-it-is-valid-blah-blah.com",
			"the-total-length@of-an-entire-address.cannot-be-longer-than-two-hundred-and-fifty-six-characters.and-this-address-is-256-characters-exactly.so-it-should-be-valid.and-im-going-to-add-some-more-words-here.to-increase-the-length-blah-blah-blah-blah-blah.org",
			"uncommon-tld@sld.mobi",
			"uncommon-tld@sld.museum",
			"uncommon-tld@sld.travel",
		};

		static readonly string[] ValidInternationalAddresses = {
			"伊昭傑@郵件.商務", // Chinese
			"राम@मोहन.ईन्फो", // Hindi
			"юзер@екзампл.ком", // Ukranian
			"θσερ@εχαμπλε.ψομ", // Greek
			"𐍈𐍈𐍈𐍈𐍈𐍈𐍈𐍈𐍈𐍈𐍈𐍈𐍈𐍈𐍈𐍈𐍈𐍈𐍈𐍈𐍈𐍈𐍈𐍈𐍈𐍈𐍈𐍈𐍈𐍈𐍈𐍈𐍈@example.com", // surrogate pair local-part
		};

		public class InvalidEmailAddress
		{
			public readonly string EmailAddress;
			public readonly EmailValidationErrorCode ErrorCode;
			public readonly int? TokenIndex;
			public readonly int? ErrorIndex;

			public InvalidEmailAddress (string emailAddress, EmailValidationErrorCode errorCode, int? tokenIndex = null, int? errorIndex = null)
			{
				EmailAddress = emailAddress;
				ErrorCode = errorCode;
				TokenIndex = tokenIndex;
				ErrorIndex = errorIndex;
			}

			public override string ToString ()
			{
				return EmailAddress;
			}
		}

		static readonly InvalidEmailAddress[] InvalidAddresses = {
			new InvalidEmailAddress ("", EmailValidationErrorCode.EmptyAddress, null, null),
			new InvalidEmailAddress ("invalid", EmailValidationErrorCode.IncompleteLocalPart, 0, 7),
			new InvalidEmailAddress ("\"invalid\"", EmailValidationErrorCode.IncompleteLocalPart, 0, 9),
			new InvalidEmailAddress ("invalid@", EmailValidationErrorCode.IncompleteDomain, 8, 8),
			new InvalidEmailAddress ("invalid @", EmailValidationErrorCode.InvalidLocalPartCharacter, 7, 7),
			new InvalidEmailAddress ("invalid@[10]", EmailValidationErrorCode.InvalidIPAddress, 9, 9),
			new InvalidEmailAddress ("invalid@[10.1]", EmailValidationErrorCode.InvalidIPAddress, 9, 9),
			new InvalidEmailAddress ("invalid@[10.1.52]", EmailValidationErrorCode.InvalidIPAddress, 9, 16),
			new InvalidEmailAddress ("invalid@[256.256.256.256]", EmailValidationErrorCode.InvalidIPAddress, 9, 12),
			new InvalidEmailAddress ("invalid@[IPv6:123456]", EmailValidationErrorCode.InvalidIPAddress, 14, 20),
			new InvalidEmailAddress ("invalid@[127.0.0.1.]", EmailValidationErrorCode.UnterminatedIPAddressLiteral, 19, 19),
			new InvalidEmailAddress ("invalid@[127.0.0.1].", EmailValidationErrorCode.UnexpectedCharactersAfterDomain, 19, 19),
			new InvalidEmailAddress ("invalid@[127.0.0.1]x", EmailValidationErrorCode.UnexpectedCharactersAfterDomain, 19, 19),
			new InvalidEmailAddress ("invalid@domain1.com@domain2.com", EmailValidationErrorCode.None, null, null),
			new InvalidEmailAddress ("\"locál-part\"@example.com", EmailValidationErrorCode.InvalidLocalPartCharacter, 4, 4), // international local-part when allowInternational=false should fail
			new InvalidEmailAddress (new string ('a', 65) + "@example.com", EmailValidationErrorCode.LocalPartTooLong, 0, 65), // local-part too long
			new InvalidEmailAddress ("invalid@" + new string ('a', 64) + ".com", EmailValidationErrorCode.DomainLabelTooLong, 8, 72), // subdomain too long
			new InvalidEmailAddress ("invalid@" + new string ('a', 60) + "." + new string ('b', 60) + "." + new string ('c', 60) + "." + new string ('d', 60) + ".com", EmailValidationErrorCode.AddressTooLong, null, null), // too long (254 characters)
			new InvalidEmailAddress ("invalid@[]", EmailValidationErrorCode.InvalidIPAddress, 9, 9), // empty IP literal
			new InvalidEmailAddress ("invalid@[192.168.10", EmailValidationErrorCode.InvalidIPAddress, 9, 19), // incomplete IPv4 literal
			new InvalidEmailAddress ("invalid@[111.111.111.111", EmailValidationErrorCode.UnterminatedIPAddressLiteral, 24, 24), // unenclosed IPv4 literal
			new InvalidEmailAddress ("invalid@[IPv6:2607:f0d0:1002:51::4", EmailValidationErrorCode.UnterminatedIPAddressLiteral, 34, 34), // unenclosed IPv6 literal
			new InvalidEmailAddress ("invalid@[IPv6:1111::1111::1111]", EmailValidationErrorCode.InvalidIPAddress, 14, 26), // invalid IPv6-comp
			new InvalidEmailAddress ("invalid@[IPv6:1111:::1111::1111]", EmailValidationErrorCode.InvalidIPAddress, 14, 21), // more than 2 consecutive :'s in IPv6
			new InvalidEmailAddress ("invalid@[IPv6:aaaa:aaaa:aaaa:aaaa:aaaa:aaaa:555.666.777.888]", EmailValidationErrorCode.InvalidIPAddress, 44, 47), // invalid IPv4 address in IPv6v4
			new InvalidEmailAddress ("invalid@[IPv6:1111:1111]", EmailValidationErrorCode.InvalidIPAddress, 14, 23), // incomplete IPv6
			new InvalidEmailAddress ("invalid@[IPv6:1::2:]", EmailValidationErrorCode.InvalidIPAddress, 14, 19), // incomplete IPv6
			new InvalidEmailAddress ("invalid@[IPv6::1::1]", EmailValidationErrorCode.InvalidIPAddress, 14, 15),
			new InvalidEmailAddress ("\"invalid-qstring@example.com", EmailValidationErrorCode.UnterminatedQuotedString, 0, 28), // unterminated q-string in local-part of the addr-spec
			new InvalidEmailAddress ("\"control-\u007f-character\"@example.com", EmailValidationErrorCode.InvalidLocalPartCharacter, 9, 9),
			new InvalidEmailAddress ("\"control-\u001f-character\"@example.com", EmailValidationErrorCode.InvalidLocalPartCharacter, 9, 9),
			new InvalidEmailAddress ("\"control-\\\u007f-character\"@example.com", EmailValidationErrorCode.InvalidLocalPartCharacter, 10, 10),

			// examples from Wikipedia
			new InvalidEmailAddress ("Abc.example.com", EmailValidationErrorCode.IncompleteLocalPart, 0, 15),
			new InvalidEmailAddress ("A@b@c@example.com", EmailValidationErrorCode.InvalidDomainCharacter, 3, 3),
			new InvalidEmailAddress ("a\"b(c)d,e:f;g<h>i[j\\k]l@example.com", EmailValidationErrorCode.InvalidLocalPartCharacter, 1, 1),
			new InvalidEmailAddress ("just\"not\"right@example.com", EmailValidationErrorCode.InvalidLocalPartCharacter, 4, 4),
			new InvalidEmailAddress ("this is\"not\\allowed@example.com", EmailValidationErrorCode.InvalidLocalPartCharacter, 4, 4),
			new InvalidEmailAddress ("this\\ still\\\"not\\\\allowed@example.com", EmailValidationErrorCode.InvalidLocalPartCharacter, 4, 4),

			// examples from https://github.com/Sembiance/email-validator
			new InvalidEmailAddress ("! #$%`|@invalid-characters-in-local.org", EmailValidationErrorCode.InvalidLocalPartCharacter, 1, 1),
			new InvalidEmailAddress ("(),:;`|@more-invalid-characters-in-local.org", EmailValidationErrorCode.InvalidLocalPartCharacter, 0, 0),
			new InvalidEmailAddress ("* .local-starts-with-dot@sld.com", EmailValidationErrorCode.InvalidLocalPartCharacter, 1, 1),
			new InvalidEmailAddress ("<>@[]`|@even-more-invalid-characters-in-local.org", EmailValidationErrorCode.InvalidLocalPartCharacter, 0, 0),
			new InvalidEmailAddress ("@missing-local.org", EmailValidationErrorCode.InvalidLocalPartCharacter, 0, 0),
			new InvalidEmailAddress ("IP-and-port@127.0.0.1:25", EmailValidationErrorCode.InvalidDomainCharacter, 21, 21),
			new InvalidEmailAddress ("another-invalid-ip@127.0.0.256", EmailValidationErrorCode.InvalidDomainCharacter, 30, 30),
			new InvalidEmailAddress ("invalid", EmailValidationErrorCode.IncompleteLocalPart, 0, 7),
			new InvalidEmailAddress ("invalid-characters-in-sld@! \"#$%(),/;<>_[]`|.org", EmailValidationErrorCode.InvalidDomainCharacter, 26, 26),
			new InvalidEmailAddress ("invalid-ip@127.0.0.1.26", EmailValidationErrorCode.InvalidDomainCharacter, 23, 23),
			new InvalidEmailAddress ("local-ends-with-dot.@sld.com", EmailValidationErrorCode.InvalidLocalPartCharacter, 20, 20),
			new InvalidEmailAddress ("missing-at-sign.net", EmailValidationErrorCode.IncompleteLocalPart, 0, 19),
			new InvalidEmailAddress ("missing-sld@.com", EmailValidationErrorCode.InvalidDomainCharacter, 12, 12),
			new InvalidEmailAddress ("missing-tld@sld.", EmailValidationErrorCode.IncompleteDomain, 12, 16),
			new InvalidEmailAddress ("sld-ends-with-dash@sld-.com", EmailValidationErrorCode.InvalidDomainCharacter, 22, 22),
			new InvalidEmailAddress ("sld-starts-with-dashsh@-sld.com", EmailValidationErrorCode.InvalidDomainCharacter, 23, 23),
			new InvalidEmailAddress ("the-character-limit@for-each-part.of-the-domain.is-sixty-four-characters.this-subdomain-is-exactly-sixty-five-characters-so-it-is-invalid1.com", EmailValidationErrorCode.DomainLabelTooLong, 73, 138),
			new InvalidEmailAddress ("the-local-part-is-invalid-if-it-is-longer-than-sixty-four-characters@sld.net", EmailValidationErrorCode.LocalPartTooLong, 0, 68),
			new InvalidEmailAddress ("the-total-length@of-an-entire-address.cannot-be-longer-than-two-hundred-and-fifty-six-characters.and-this-address-is-257-characters-exactly.so-it-should-be-invalid.lets-add-some-extra-words-here.to-increase-the-length.beyond-the-256-character-limitation.org", EmailValidationErrorCode.AddressTooLong, null, null),
			new InvalidEmailAddress ("two..consecutive-dots@sld.com", EmailValidationErrorCode.InvalidLocalPartCharacter, 4, 4),
			new InvalidEmailAddress ("unbracketed-IP@127.0.0.1", EmailValidationErrorCode.IncompleteDomainLabel, 23, 24),
			new InvalidEmailAddress ("dot-first-in-domain@.test.de", EmailValidationErrorCode.InvalidDomainCharacter, 20, 20),
			new InvalidEmailAddress ("single-character-tld@ns.i", EmailValidationErrorCode.IncompleteDomainLabel, 24, 25),

			// examples of real (invalid) input from real users.
			new InvalidEmailAddress ("No longer available.", EmailValidationErrorCode.InvalidLocalPartCharacter, 2, 2),
			new InvalidEmailAddress ("Moved.", EmailValidationErrorCode.IncompleteLocalPart, 0, 6)
		};

		static readonly InvalidEmailAddress[] InvalidInternationalAddresses = {
			new InvalidEmailAddress ("test@𐍈", EmailValidationErrorCode.IncompleteDomainLabel, 5, 7), // single "character" surrogate-pair domain
		};

		[TestCaseSource (nameof (ValidAddresses))]
		public void TestValidAddresses (string email)
		{
			Assert.That (EmailValidator.Validate (email, true, false), Is.True, "Validate");
			Assert.That (EmailValidator.TryValidate (email, true, false, out _), Is.True, "TryValidate");
		}

		[TestCaseSource (nameof (InvalidAddresses))]
		public void TestInvalidAddresses (InvalidEmailAddress invalid)
		{
			Assert.That (EmailValidator.Validate (invalid.EmailAddress, true, false), Is.False, "Validate");
			Assert.That (EmailValidator.TryValidate (invalid.EmailAddress, true, false, out var error), Is.False, "TryValidate");
			Assert.That (error.Code, Is.EqualTo (invalid.ErrorCode), "ErrorCode");
			Assert.That (error.TokenIndex, Is.EqualTo (invalid.TokenIndex));
			Assert.That (error.ErrorIndex, Is.EqualTo (invalid.ErrorIndex));
		}

		[Test]
		public void TestInvalidAddressTopLevelDomain ()
		{
			Assert.That (EmailValidator.Validate ("invalid@tld"), Is.False, "Validate");
			Assert.That (EmailValidator.TryValidate ("invalid@tld", false, false, out var error), Is.False, "TryValidate");
			Assert.That (error.Code, Is.EqualTo (EmailValidationErrorCode.IncompleteDomain));
			Assert.That (error.TokenIndex, Is.EqualTo (8));
			Assert.That (error.ErrorIndex, Is.EqualTo (11));
		}

		[TestCaseSource (nameof (ValidInternationalAddresses))]
		public void TestValidInternationalAddresses (string email)
		{
			Assert.That (EmailValidator.Validate (email, true, true), Is.True, "Validate");
			Assert.That (EmailValidator.TryValidate (email, true, true, out _), Is.True, "TryValidate");
		}

		[TestCaseSource (nameof (InvalidInternationalAddresses))]
		public void TestInvalidInternationalAddresses (InvalidEmailAddress invalid)
		{
			Assert.That (EmailValidator.Validate (invalid.EmailAddress, true, true), Is.False, "Validate");
			Assert.That (EmailValidator.TryValidate (invalid.EmailAddress, true, true, out var error), Is.False, "TryValidate");
			Assert.That (error.Code, Is.EqualTo (invalid.ErrorCode), "ErrorCode");
			Assert.That (error.TokenIndex, Is.EqualTo (invalid.TokenIndex));
			Assert.That (error.ErrorIndex, Is.EqualTo (invalid.ErrorIndex));
		}

		[Test]
		public void TestArgumentNullException ()
		{
			Assert.Throws<ArgumentNullException> (() => EmailValidator.Validate (null, true, true), "Validate null Address");
			Assert.Throws<ArgumentNullException> (() => EmailValidator.TryValidate (null, true, true, out _), "TryValidate null Address");
		}

		[TestCaseSource (nameof (ValidAddresses))]
		public void TestValidationAttributeValidAddresses (string email)
		{
			var target = new EmailValidationTarget () {
				Email = email
			};

			Assert.That (AreAttributesValid (target), Is.True);
		}

		[TestCaseSource (nameof (InvalidAddresses))]
		public void TestValidationAttributeInvalidAddresses (InvalidEmailAddress invalid)
		{
			var target = new EmailValidationTarget () {
				Email = invalid.EmailAddress
			};

			Assert.That (AreAttributesValid (target), Is.False);
		}

		[TestCaseSource (nameof (ValidInternationalAddresses))]
		public void TestValidationAttributeValidInternationalAddresses (string email)
		{
			var target = new InternationalEmailValidationTarget () {
				Email = email
			};

			Assert.That (AreAttributesValid (target), Is.True);
		}

		[TestCaseSource (nameof (InvalidInternationalAddresses))]
		public void TestValidationAttributeInvalidInternationalAddresses (InvalidEmailAddress invalid)
		{
			var target = new InternationalEmailValidationTarget () {
				Email = invalid.EmailAddress
			};

			Assert.That (AreAttributesValid (target), Is.False);
		}

		[Test]
		public void TestValidationAttributeNullEmail ()
		{
			var target = new EmailValidationTarget () {
				Email = null
			};

			Assert.That (AreAttributesValid (target), Is.True, "Email is allowed to be null");
		}

		static bool AreAttributesValid (object target)
		{
			var context = new ValidationContext (target, null, null);
			var results = new List<ValidationResult> ();

			return Validator.TryValidateObject (target, context, results, true);
		}

		class EmailValidationTarget
		{
			[Email (true)]
			public string Email { get; set; }
		}

		class InternationalEmailValidationTarget
		{
			[Email (true, true)]
			public string Email { get; set; }
		}
	}
}
