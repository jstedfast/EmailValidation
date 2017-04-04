//
// Test.cs
//
// Author: Jeffrey Stedfast <jestedfa@microsoft.com>
//
// Copyright (c) 2013-2017 Jeffrey Stedfast
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
using NUnit.Framework;
using EmailValidation;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace UnitTests
{
	[TestFixture]
	public class Test
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
			"valid.ipv4.addr@[123.1.72.10]",
			"valid.ipv6.addr@[IPv6:0::1]",
			"valid.ipv6.addr@[IPv6:2607:f0d0:1002:51::4]",
			"valid.ipv6.addr@[IPv6:fe80::230:48ff:fe33:bc33]",
			"valid.ipv6.addr@[IPv6:fe80:0000:0000:0000:0202:b3ff:fe1e:8329]",
			"valid.ipv6v4.addr@[IPv6:aaaa:aaaa:aaaa:aaaa:aaaa:aaaa:127.0.0.1]",

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
			"the-total-length@of-an-entire-address.cannot-be-longer-than-two-hundred-and-fifty-four-characters.and-this-address-is-254-characters-exactly.so-it-should-be-valid.and-im-going-to-add-some-more-words-here.to-increase-the-length-blah-blah-blah-blah-bla.org",
			"uncommon-tld@sld.mobi",
			"uncommon-tld@sld.museum",
			"uncommon-tld@sld.travel",
		};

		static readonly string[] InvalidAddresses = {
			"",
			"invalid",
			"invalid@",
			"invalid @",
			"invalid@[555.666.777.888]",
			"invalid@[IPv6:123456]",
			"invalid@[127.0.0.1.]",
			"invalid@[127.0.0.1].",
			"invalid@[127.0.0.1]x",

			// examples from wikipedia
			"Abc.example.com",
			"A@b@c@example.com",
			"a\"b(c)d,e:f;g<h>i[j\\k]l@example.com",
			"just\"not\"right@example.com",
			"this is\"not\\allowed@example.com",
			"this\\ still\\\"not\\\\allowed@example.com",

			// examples from https://github.com/Sembiance/email-validator
			"! #$%`|@invalid-characters-in-local.org",
			"(),:;`|@more-invalid-characters-in-local.org",
			"* .local-starts-with-dot@sld.com",
			"<>@[]`|@even-more-invalid-characters-in-local.org",
			"@missing-local.org",
			"IP-and-port@127.0.0.1:25",
			"another-invalid-ip@127.0.0.256",
			"invalid",
			"invalid-characters-in-sld@! \"#$%(),/;<>_[]`|.org",
			"invalid-ip@127.0.0.1.26",
			"local-ends-with-dot.@sld.com",
			"missing-at-sign.net",
			"missing-sld@.com",
			"missing-tld@sld.",
			"sld-ends-with-dash@sld-.com",
			"sld-starts-with-dashsh@-sld.com",
			"the-character-limit@for-each-part.of-the-domain.is-sixty-three-characters.this-is-exactly-sixty-four-characters-so-it-is-invalid-blah-blah.com",
			"the-local-part-is-invalid-if-it-is-longer-than-sixty-four-characters@sld.net",
			"the-total-length@of-an-entire-address.cannot-be-longer-than-two-hundred-and-fifty-four-characters.and-this-address-is-255-characters-exactly.so-it-should-be-invalid.and-im-going-to-add-some-more-words-here.to-increase-the-lenght-blah-blah-blah-blah-bl.org",
			"two..consecutive-dots@sld.com",
			"unbracketed-IP@127.0.0.1",

			// examples of real (invalid) input from real users.
			"No longer available.",
			"Moved."
		};

		static readonly string[] ValidInternationalAddresses = {
			"伊昭傑@郵件.商務",    // Chinese
			"राम@मोहन.ईन्फो",       // Hindi
			"юзер@екзампл.ком", // Ukranian
			"θσερ@εχαμπλε.ψομ", // Greek
		};

		[Test]
		public void TestValidAddresses ()
		{
			for (int i = 0; i < ValidAddresses.Length; i++)
				Assert.IsTrue (EmailValidator.Validate (ValidAddresses[i], true), "Valid Address #{0}: {1}", i, ValidAddresses[i]);
		}

		[Test]
		public void TestInvalidAddresses ()
		{
			for (int i = 0; i < InvalidAddresses.Length; i++)
				Assert.IsFalse (EmailValidator.Validate (InvalidAddresses[i], true), "Invalid Address #{0}: {1}", i, InvalidAddresses[i]);
		}

		[Test]
		public void TestValidInternationalAddresses ()
		{
			for (int i = 0; i < ValidInternationalAddresses.Length; i++)
				Assert.IsTrue (EmailValidator.Validate (ValidInternationalAddresses[i], true, true), "Valid International Address #{0}", i);
		}

		[Test]
		public void TestThrowsExceptionIfNull ()
		{
			Assert.Throws<ArgumentNullException> (() => EmailValidator.Validate (null, true, true), "Null Address");
		}

		[Test]
		public void TestValidationAttributeValidAddresses ()
		{
			EmailValidationTarget target = new EmailValidationTarget ();

			foreach (var email in ValidAddresses) {
				target.Email = email;

				Assert.IsTrue (AreAttributesValid (target), "Valid Address {0}", email);
			}
		}

		[Test]
		public void TestValidationAttributeInvalidAddresses ()
		{
			EmailValidationTarget target = new EmailValidationTarget ();

			foreach (var email in InvalidAddresses) {
				target.Email = email;

				Assert.IsFalse (AreAttributesValid (target), "Invalid Address {0}", email);
			}
		}

		[Test]
		public void TestValidationAttributeValidInternationalAddresses ()
		{
			var target = new InternationalEmailValidationTarget ();

			foreach (var email in ValidInternationalAddresses) {
				target.Email = email;

				Assert.IsTrue (AreAttributesValid (target), "Valid International Address {0}", email);
			}
		}

		bool AreAttributesValid (object target)
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
