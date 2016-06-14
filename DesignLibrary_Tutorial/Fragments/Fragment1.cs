using System;
using Android.App;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using SupportFragment = Android.Support.V4.App.Fragment;
using System.Collections.Generic;
using DesignLibrary_Tutorial.Helpers;
using Android.Graphics;
using Android.Util;
using Android.Content;
using Android.Content.Res;
using Android.Widget;
using DesignLibrary.Helpers;
using DesignLibrary_Tutorial.Resources;
using DesignLibrary_Tutorial.Classes;

namespace DesignLibrary_Tutorial.Fragments
{
    public class Fragment1 : SupportFragment
    {
       
        private FrameLayout Fragment1Frame;
        private LinearLayout Menu;
        private LayoutInflater Inflater;
        private ViewGroup Container;
        private View CurrentView;

        private Quiz Quiz4TScore;



        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            Inflater = inflater;
            Container = container;

            Container.SetPadding(0, 0, 0, 220);

            CurrentView = Inflater.Inflate(Resource.Layout.Fragment1, Container, false);

            

            Button btn_4Ts = CurrentView.FindViewById<Button>(Resource.Id.btn_4Ts);
            Button btn_HEP = CurrentView.FindViewById<Button>(Resource.Id.btn_HEP);
            Menu = CurrentView.FindViewById<LinearLayout>(Resource.Id.Menu);
            Fragment1Frame = CurrentView.FindViewById<FrameLayout>(Resource.Id.Fragment1Frame);


            btn_4Ts.Click += delegate { Start4Ts(); };

            return CurrentView;
        }

        private void Start4Ts()
        {

            Quiz4TScore = new Quiz();

            //Question 1
            Question Question1 = new Question();
            Question1.Issue = "Platelet nadir ?";
            Question1.Choices.Add(new Choice("≥20 G/L", 2, 2));
            Question1.Choices.Add(new Choice("10-19 G/L", 1, 1));
            Question1.Choices.Add(new Choice("<10 G/L", 0, 1));
            Quiz4TScore.Questions.Add(Question1);

            //Question 2
            Question Question2 = new Question();
            Question2.Issue = "Platelet count fall ?";
            Question2.Choices.Add(new Choice("> 50 %", 1, 2));
            Question2.Choices.Add(new Choice("< 50 %", 0, 2));
            Quiz4TScore.Questions.Add(Question2);

            //Question 3
            Question Question3 = new Question();
            Question3.Issue = "Timing of platelet count fall ?";
            Question3.Choices.Add(new Choice("Clear onset between days 5-10 without recent heparin exposure", 2, 3));
            Question3.Choices.Add(new Choice("≤1 day with prior heparin exposure within 30 days", 2, 3));
            Question3.Choices.Add(new Choice("Consistent with day 5-10 fall but not clear (missing platelet count)", 1, 3));
            Question3.Choices.Add(new Choice("Onset after day 10", 1, 3));
            Question3.Choices.Add(new Choice("Fall ≤1 day prior exposure 30-100 days", 1, 3));
            Question3.Choices.Add(new Choice("<4 days without recent heparin exposure", 0, 3));
            Quiz4TScore.Questions.Add(Question3);

            //Question 4
            Question Question4 = new Question();
            Question4.Issue = "Thrombosis or other sequelae ?";
            Question4.Choices.Add(new Choice("New thrombosis confirmed", 2, 4));
            Question4.Choices.Add(new Choice("Skin necronis at heparin injection sites", 2, 4));
            Question4.Choices.Add(new Choice("Acute systemic reaction after intravenous heparin bolus", 2, 4));
            Question4.Choices.Add(new Choice("Progressive or recurrent thrombosis", 1, 4));
            Question4.Choices.Add(new Choice("Non-necrotising (erythematous) skin lesions", 1, 4));
            Question4.Choices.Add(new Choice("Suspected thrombosis (not proven)", 1, 4));
            Question4.Choices.Add(new Choice("None", 0, 4));
            Quiz4TScore.Questions.Add(Question4);

            //Question 5
            Question Question5 = new Question();
            Question5.Issue = "Other cause for thrombocytopenia ?";
            Question5.Choices.Add(new Choice("None apparent", 2, -1));
            Question5.Choices.Add(new Choice("Possible", 1, -1));
            Question5.Choices.Add(new Choice("Definite", 0, -1));
            Quiz4TScore.Questions.Add(Question5);


            MenuAnimation MenuFadeOut = new MenuAnimation(Menu, 500);

            MenuFadeOut.AnimationEnd += StartQuiz;
            //MenuFadeOut.AnimationStart += DisplayNextQuestion;
            Menu.StartAnimation(MenuFadeOut);
        }

        void StartQuiz(object sender, Android.Views.Animations.Animation.AnimationEndEventArgs e)
        {

            DisplayNextQuestion();

        }

        void DisplayNextQuestion()
        {
            CurrentView = Inflater.Inflate(Resource.Layout.QuestionLayout, Container, false);
            Fragment1Frame.RemoveAllViews();
            Fragment1Frame.AddView(CurrentView);


            TextView IssueTextView = CurrentView.FindViewById<TextView>(Resource.Id.TextViewQuestion);
            ListView ChoicesListView = CurrentView.FindViewById<ListView>(Resource.Id.ListViewChoices);


            IssueTextView.Text = Quiz4TScore.CurrentQuestion.Issue;
            ArrayAdapter ChoicesListViewAdapter = new ArrayAdapter(this.Context, Android.Resource.Layout.SimpleListItem1, Quiz4TScore.CurrentQuestion.ChoicesTexts);
            ChoicesListView.Adapter = ChoicesListViewAdapter;

            ChoicesListView.ItemClick += ChoiceClick;

        }

        void ChoiceClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            
            if (Quiz4TScore.CurrentQuestion.Choices[e.Position].NextQuestionId != -1) //-1 means it was the last question
            {
                Quiz4TScore.AnswerQuestion(e.Position);
                DisplayNextQuestion();
            }
            else
            {
                Quiz4TScore.AnswerQuestion(e.Position);
                DisplayResult();
            }
        }


        void DisplayResult()
        {
            int final_score = Quiz4TScore.QuizResult._score;

            CurrentView = Inflater.Inflate(Resource.Layout.ResultLayout, Container, false);
            Fragment1Frame.RemoveAllViews();
            Fragment1Frame.AddView(CurrentView);

            TextView ScoreTextView = CurrentView.FindViewById<TextView>(Resource.Id.TextViewFinalScore);
            TextView TextViewFinalScoreDetail = CurrentView.FindViewById<TextView>(Resource.Id.TextViewFinalScoreDetail);
            Button BtnRestart = CurrentView.FindViewById<Button>(Resource.Id.BtnRestart);
            Button BtnExit = CurrentView.FindViewById<Button>(Resource.Id.BtnExit);

            ScoreTextView.Text = final_score.ToString();

            String Result;
            
            if (final_score <= 3)
            {
                Result = "Hit is unlikely. No change in heparin treatment.";
            }
            else if (final_score <= 5)
            {
                Result = "The risk of HIT is intermediate. Ask for an immunoassay.";
            }
            else
            {
                Result = "The risk of HIT is high. Ask for an immunoassay.";
            }
            TextViewFinalScoreDetail.Text = Result;

            BtnRestart.Click += delegate { Quiz4TScore._QuizHistory.FullHistory.Clear(); DisplayNextQuestion(); };
            BtnExit.Click += delegate { Android.OS.Process.KillProcess(Android.OS.Process.MyPid()); };
        }

    }























    //public class Fragment1 : SupportFragment
    //{
    //    public override void OnCreate(Bundle savedInstanceState)
    //    {
    //        base.OnCreate(savedInstanceState);

    //        // Create your fragment here
    //    }

    //    public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
    //    {
    //        RecyclerView recyclerView = inflater.Inflate(Resource.Layout.Fragment1, container, false) as RecyclerView;

    //        SetUpRecyclerView(recyclerView);

    //        return recyclerView;
    //    }

    //    private void SetUpRecyclerView(RecyclerView recyclerView)
    //    {
    //        var values = GetRandomSubList(Cheeses.CheeseStrings, 3);

    //        recyclerView.SetLayoutManager(new LinearLayoutManager(recyclerView.Context));
    //        recyclerView.SetAdapter(new SimpleStringRecyclerViewAdapter(recyclerView.Context, values, Activity.Resources));

    //        recyclerView.SetItemClickListener((rv, position, view) =>
    //        {
    //            //An item has been clicked
    //            Context context = view.Context;
    //            Intent intent = new Intent(context, typeof(CheeseDetailActivity));
    //            intent.PutExtra(CheeseDetailActivity.EXTRA_NAME, values[position]);

    //            context.StartActivity(intent);
    //        });
    //    }

    //    private List<string> GetRandomSubList (List<string> items, int amount)
    //    {
    //        List<string> list = new List<string>();
    //        Random random = new Random();
    //        while (list.Count < amount)
    //        {
    //            list.Add(items[random.Next(items.Count)]);
    //        }

    //        return list;
    //    }

    //    public class SimpleStringRecyclerViewAdapter : RecyclerView.Adapter
    //    {
    //        private readonly TypedValue mTypedValue = new TypedValue();
    //        private int mBackground;
    //        private List<string> mValues;
    //        Resources mResource;
    //        private Dictionary<int, int> mCalculatedSizes;

    //        public SimpleStringRecyclerViewAdapter(Context context, List<string> items, Resources res)
    //        {
    //            context.Theme.ResolveAttribute(Resource.Attribute.selectableItemBackground, mTypedValue, true);
    //            mBackground = mTypedValue.ResourceId;
    //            mValues = items;
    //            mResource = res;

    //            mCalculatedSizes = new Dictionary<int, int>();
    //        }

    //        public override int ItemCount
    //        {
    //            get
    //            {
    //                return mValues.Count;
    //            }
    //        }

    //        public override async void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
    //        {
    //            var simpleHolder = holder as SimpleViewHolder;

    //            simpleHolder.mBoundString = mValues[position];
    //            simpleHolder.mTxtView.Text = mValues[position];

    //            int drawableID = Cheeses.RandomCheeseDrawable;
    //            BitmapFactory.Options options = new BitmapFactory.Options();

    //            if (mCalculatedSizes.ContainsKey(drawableID))
    //            {
    //                options.InSampleSize = mCalculatedSizes[drawableID];
    //            }

    //            else
    //            {
    //                options.InJustDecodeBounds = true;

    //                BitmapFactory.DecodeResource(mResource, drawableID, options);

    //                options.InSampleSize = Cheeses.CalculateInSampleSize(options, 100, 100);
    //                options.InJustDecodeBounds = false;

    //                mCalculatedSizes.Add(drawableID, options.InSampleSize);
    //            }


    //            var bitMap = await BitmapFactory.DecodeResourceAsync(mResource, drawableID, options);

    //            simpleHolder.mImageView.SetImageBitmap(bitMap);
    //        }

    //        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
    //        {
    //            View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.List_Item, parent, false);
    //            view.SetBackgroundResource(mBackground);

    //            return new SimpleViewHolder(view);
    //        }
    //    }

    //    public class SimpleViewHolder : RecyclerView.ViewHolder
    //    {
    //        public string mBoundString;
    //        public readonly View mView;
    //        public readonly ImageView mImageView;
    //        public readonly TextView mTxtView;

    //        public SimpleViewHolder(View view) : base(view)
    //        {
    //            mView = view;
    //            mImageView = view.FindViewById<ImageView>(Resource.Id.avatar);
    //            mTxtView = view.FindViewById<TextView>(Resource.Id.text1);
    //        }

    //        public override string ToString()
    //        {
    //            return base.ToString() + " '" + mTxtView.Text;
    //        }
    //    }
    //}
}