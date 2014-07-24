//
// Test.cs
//
// Author: Jeffrey Stedfast <jeff@xamarin.com>
//
// Copyright (c) 2013 Xamarin Inc.
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
			"\" \"@example.org"
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
			"this\\ still\\\"not\\\\allowed@example.com"
		};

		static readonly string[] ValidInternationalAddresses = {
			"伊昭傑@郵件.商務",    // Chinese
			"राम@मोहन.ईन्फो",       // Hindu
			"юзер@екзампл.ком", // Ukranian
			"θσερ@εχαμπλε.ψομ", // Greek
		};

		[Test]
		public void TestValidAddresses ()
		{
			for (int i = 0; i < ValidAddresses.Length; i++)
				Assert.IsTrue (EmailValidator.Validate (ValidAddresses[i]), "Valid Address #{0}", i);
		}

		[Test]
		public void TestInvalidAddresses ()
		{
			for (int i = 0; i < InvalidAddresses.Length; i++)
				Assert.IsFalse (EmailValidator.Validate (InvalidAddresses[i]), "Invalid Address #{0}", i);
		}

		[Test]
		public void TestValidInternationalAddresses ()
		{
			for (int i = 0; i < ValidInternationalAddresses.Length; i++)
				Assert.IsTrue (EmailValidator.Validate (ValidInternationalAddresses[i], true), "Valid International Address #{0}", i);
		}
	}
}
