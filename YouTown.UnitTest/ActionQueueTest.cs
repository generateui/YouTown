using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using YouTown.GameAction;

namespace YouTown.UnitTest
{
    [TestClass]
    public class ActionQueueTest
    {
        [TestMethod]
        public void OptionalAction_DoesNotBlockOtherAction()
        {
            var queue = new ActionQueue();
            var queuedAction = new Mock<IGameAction>().Object;
            queue.EnqueueSingle(queuedAction, optional: true);

            var actionToPlay = new Mock<IGameAction>().Object;
            bool satisfies = queue.Satisfies(actionToPlay, mustBePresent: false);
            Assert.IsTrue(satisfies);
        }

        [TestMethod]
        public void NonOptionalAction_BlocksOtherActionWithSamePlayer()
        {
            var queue = new ActionQueue();
            var player = new Mock<IPlayer>().Object;

            var queuedActionMock = new Mock<IGameAction>();
            var actionTestType1 = new ActionType("test1");
            queuedActionMock.Setup(ga => ga.ActionType).Returns(actionTestType1);
            var queuedAction = queuedActionMock.Object;

            var actionToPlayMock = new Mock<IGameAction>();
            var actionTestType2 = new ActionType("test2");
            actionToPlayMock.Setup(ga => ga.ActionType).Returns(actionTestType2);
            actionToPlayMock.Setup(ga => ga.Player).Returns(player);
            var actionToPlay = actionToPlayMock.Object;

            queue.EnqueueSingle(queuedAction, optional: false);

            bool satisfies = queue.Satisfies(actionToPlay, mustBePresent: false);
            Assert.IsFalse(satisfies);
        }

        [TestMethod]
        public void OrderedActions_UnblockQueueOnlyAfterPerformed()
        {
            var queue = new ActionQueue();
            var player = new Mock<IPlayer>().Object;

            var rogueActionMock = new Mock<IGameAction>();
            var rogueActionType = new ActionType("rogue");
            rogueActionMock.Setup(ga => ga.ActionType).Returns(rogueActionType);
            var rogueAction = rogueActionMock.Object;

            var actionToPlayMock1 = new Mock<IGameAction>();
            var actionType1 = new ActionType("action1");
            actionToPlayMock1.Setup(ga => ga.ActionType).Returns(actionType1);
            actionToPlayMock1.Setup(ga => ga.Player).Returns(player);
            var actionToPlay1 = actionToPlayMock1.Object;

            var actionToPlayMock2 = new Mock<IGameAction>();
            var actionType2 = new ActionType("action2");
            actionToPlayMock2.Setup(ga => ga.ActionType).Returns(actionType2);
            actionToPlayMock2.Setup(ga => ga.Player).Returns(player);
            var actionToPlay2 = actionToPlayMock2.Object;

            queue.EnqueueOrdered(new List<IGameAction> { actionToPlay1, actionToPlay2});

            Assert.IsFalse(queue.Satisfies(rogueAction));
            Assert.IsTrue(queue.Satisfies(actionToPlay1));
            Assert.IsFalse(queue.Satisfies(actionToPlay2));

            queue.Dequeue(actionToPlay1);
            Assert.IsFalse(queue.Satisfies(rogueAction));
            Assert.IsFalse(queue.Satisfies(actionToPlay1));
            Assert.IsTrue(queue.Satisfies(actionToPlay2));

            queue.Dequeue(actionToPlay2);
            Assert.IsTrue(queue.Satisfies(rogueAction));
        }


        [TestMethod]
        public void UnorderedActions_UnblockQueueOnlyAfterPerformed()
        {
            var queue = new ActionQueue();
            var player = new Mock<IPlayer>().Object;

            var rogueActionMock = new Mock<IGameAction>();
            var rogueActionType = new ActionType("rogue");
            rogueActionMock.Setup(ga => ga.ActionType).Returns(rogueActionType);
            var rogueAction = rogueActionMock.Object;

            var actionToPlayMock1 = new Mock<IGameAction>();
            var actionType1 = new ActionType("action1");
            actionToPlayMock1.Setup(ga => ga.ActionType).Returns(actionType1);
            actionToPlayMock1.Setup(ga => ga.Player).Returns(player);
            var actionToPlay1 = actionToPlayMock1.Object;

            var actionToPlayMock2 = new Mock<IGameAction>();
            var actionType2 = new ActionType("action2");
            actionToPlayMock2.Setup(ga => ga.ActionType).Returns(actionType2);
            actionToPlayMock2.Setup(ga => ga.Player).Returns(player);
            var actionToPlay2 = actionToPlayMock2.Object;

            queue.EnqueueUnordered(new List<IGameAction> { actionToPlay1, actionToPlay2 });

            Assert.IsFalse(queue.Satisfies(rogueAction));
            Assert.IsTrue(queue.Satisfies(actionToPlay1));
            Assert.IsTrue(queue.Satisfies(actionToPlay2));

            queue.Dequeue(actionToPlay2);
            Assert.IsFalse(queue.Satisfies(rogueAction));
            Assert.IsTrue(queue.Satisfies(actionToPlay1));
            Assert.IsFalse(queue.Satisfies(actionToPlay2));

            queue.Dequeue(actionToPlay1);
            Assert.IsTrue(queue.Satisfies(rogueAction));
        }
    }
}
