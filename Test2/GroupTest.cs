﻿using System.Linq;
using Domain2;
using Domain2.Utils;
using NUnit.Framework;

namespace Test2
{

    [TestFixture]
    public class GroupTest
    {
        [Test]
        public void test_create_payment_notifications()
        {

            var administrator = new Member();

            var memberOne = new Member();

            var newGroup = administrator.createGroup("GroupOne");

            memberOne.joinGroup(newGroup);

            var newPayment = new Payment() { amount = 120f, collector = administrator, debtor = memberOne };

            newGroup.createPaymentNotification(newPayment);

            Assert.AreEqual(1, administrator.notifications.Count);

            Assert.AreEqual(NotificationStatus.Unread, administrator.notifications.ElementAt(0).status);

            Assert.AreEqual(0, memberOne.notifications.Count);

        }
        [Test]
        public void test_create_purchase_notifications()
        {
            var administrator = new Member();

            var memberOne = new Member();

            var memberTwo = new Member();

            var newGroup = administrator.createGroup("GroupOne");

            memberOne.joinGroup(newGroup);

            memberTwo.joinGroup(newGroup);

            var newPurchase = new Purchase() { buyer = administrator, debtors = newGroup.members, description = "Glass", totalAmount = 30f, @group = newGroup };

            newGroup.createPurchaseNotifications(newPurchase);

            Assert.AreEqual(1, memberOne.notifications.Count);

            Assert.AreEqual(NotificationStatus.Unread, memberOne.notifications.ElementAt(0).status);

            Assert.AreEqual(1, memberTwo.notifications.Count);

            Assert.AreEqual(NotificationStatus.Unread, memberTwo.notifications.ElementAt(0).status);

            Assert.AreEqual(0, administrator.notifications.Count);

        }
    }

}