﻿using System.Linq;
using Domain2;
using Domain2.Exceptions;
using Domain2.Utils;
using NUnit.Framework;

namespace Test2
{
    [TestFixture]
    public class MemberTest
    {

        [Test]
        public void test_new_member_calculate_owed_amounts()
        {

            Assert.AreEqual(0f, new Member().getOwedAmount());

        }

        [Test]
        public void test_if_new_member_has_debts()
        {

            Assert.IsFalse(new Member().hasDebts());

        }

        [Test]
        public void test_one_member_creates_one_group()
        {

            var administrator = new Member() { name = "Irene", lastName = "Smith" };

            var newGroup = administrator.createGroup("GroupOne");

            Assert.AreEqual(1, administrator.groups.Count);

            Assert.AreEqual("GroupOne", administrator.groups.ElementAt(0).name);

            Assert.AreEqual("Irene", newGroup.administrator.name);

            Assert.AreEqual("Smith", newGroup.administrator.lastName);

        }

        [Test]
        public void test_one_member_creates_two_groups()
        {

            var administrator = new Member() { name = "Irene", lastName = "Smith" };

            var newGroupOne = administrator.createGroup("GroupOne");

            var newGroupTwo = administrator.createGroup("GroupTwo");

            Assert.AreEqual(2, administrator.groups.Count);

            Assert.AreEqual("GroupOne", administrator.groups.ElementAt(0).name);

            Assert.AreEqual("Irene", newGroupOne.administrator.name);

            Assert.AreEqual("Smith", newGroupOne.administrator.lastName);

            Assert.AreEqual("GroupTwo", administrator.groups.ElementAt(1).name);

            Assert.AreEqual("Irene", newGroupTwo.administrator.name);

            Assert.AreEqual("Smith", newGroupTwo.administrator.lastName);

        }

        [Test]
        public void test_one_member_joins_a_group()
        {

            var administrator = new Member();

            var memberOne = new Member() { name = "Walter", lastName = "Placona" };

            var newGroup = administrator.createGroup("GroupOne");

            memberOne.joinGroup(newGroup);

            Assert.AreEqual(1, memberOne.groups.Count);

            Assert.AreEqual("GroupOne", memberOne.groups.ElementAt(0).name);

            Assert.AreEqual(1, newGroup.members.Count);

        }

        [Test]
        public void test_one_member_joins_two_groups()
        {

            var administrator = new Member();

            var memberOne = new Member() { name = "Walter", lastName = "Placona" };

            var newGroupOne = administrator.createGroup("GroupOne");

            var newGroupTwo = administrator.createGroup("GroupTwo");

            memberOne.joinGroup(newGroupOne);

            memberOne.joinGroup(newGroupTwo);

            Assert.AreEqual(2, memberOne.groups.Count);

            Assert.AreEqual("GroupOne", memberOne.groups.ElementAt(0).name);

            Assert.AreEqual("GroupTwo", memberOne.groups.ElementAt(0).name);

            Assert.AreEqual(1, newGroupOne.members.Count);

            Assert.AreEqual(1, newGroupTwo.members.Count);

        }

        [Test]
        [ExpectedException(typeof(AlreadyJoinedException))]
        public void test_one_member_tries_to_join_twice_one_group()
        {

            var administrator = new Member();

            var memberOne = new Member();

            var newGroup = administrator.createGroup("GroupOne");

            memberOne.joinGroup(newGroup);

            memberOne.joinGroup(newGroup);

        }

        [Test]
        [ExpectedException(typeof(AlreadyJoinedException))]
        public void test_one_administrator_tries_to_join_group()
        {

            var administrator = new Member();

            var newGroup = administrator.createGroup("GroupOne");

            administrator.joinGroup(newGroup);

        }

        [Test]
        public void test_one_member_leaves_group()
        {

            var administrator = new Member();

            var memberOne = new Member();

            var newGroup = administrator.createGroup("GroupOne");

            memberOne.joinGroup(newGroup);

            memberOne.leaveGroup(newGroup);

            Assert.AreEqual(0, memberOne.groups.Count);

            Assert.AreEqual(0, newGroup.members.Count);

        }

        [Test]
        [ExpectedException(typeof(NotJoinedException))]
        public void test_one_unjoined_member_tries_to_leave_group()
        {

            var administrator = new Member();

            var memberOne = new Member();

            var newGroup = administrator.createGroup("GroupOne");

            memberOne.leaveGroup(newGroup);

        }

        [Test]
        public void test_one_member_register_purchase()
        {

            var administrator = new Member();

            var memberOne = new Member();

            var memberTwo = new Member();

            var newGroup = administrator.createGroup("GroupOne");

            memberOne.joinGroup(newGroup);

            memberTwo.joinGroup(newGroup);

            var newPurchase = new Purchase()
            {
                buyer = administrator,
                debtors = newGroup.members,
                description = "Pelota",
                @group = newGroup,
                totalAmount = 120f
            };

            administrator.registerPurchase(newPurchase);

            Assert.AreEqual(40f, memberOne.payments.ElementAt(0).amount);

            Assert.AreEqual(40f, memberTwo.payments.ElementAt(0).amount);

            Assert.AreEqual(PaymentStatus.Paid, administrator.payments.ElementAt(0).status);

            Assert.AreEqual(PaymentStatus.Unpaid, memberOne.payments.ElementAt(0).status);

            Assert.AreEqual(PaymentStatus.Unpaid, memberTwo.payments.ElementAt(0).status);

        }

    }
}